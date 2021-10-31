using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BranchController : BaseApiController
    {
        private readonly FirebaseDataContext _firebaseDataContext;
        private IPhotoService _photoService;
        public BranchController(IPhotoService photoService)
        {
            _firebaseDataContext = new FirebaseDataContext();
            _photoService = photoService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<BranchDto>> CreateBranch(BranchDto dto)
        {
            List<Branch> branches = new List<Branch>();
            branches = await GetBranches2();

            for (int i = 0; i < branches.Count; i++)
            {
                var item = branches[i];

                if (item.Id == dto.Id)
                {
                    while (item.Id == dto.Id)
                    {
                        dto.Id = "rd" + new Random().Next(100000, 999999);
                        i = 0;
                    }
                }
            }

            Branch branch = new Branch()
            {
                Id = dto.Id,
                Location = dto.Location,
                Name = dto.Name
            };

            string path = dto.Img;
            int n = path.IndexOf("base64,");

            path = path.Remove(0, n + 7);
            var bytes = Convert.FromBase64String(path);

            if (path != null)
            {
                FormFile file;

                var stream = new MemoryStream(bytes);
                file = new FormFile(stream, 0, stream.Length, null, "foodName");

                var result = await _photoService.AddPhotoAsync(file);

                if (result.Error != null) return BadRequest(result.Error.Message);

                branch.ImgUrl = result.SecureUrl.AbsoluteUri;
                branch.PublicId = result.PublicId;
            }        

            _firebaseDataContext.StoreData("Branch/" + dto.Id, branch);

            return dto;
        }
        [HttpGet("getbranches")]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetBranches()
        {
            List<BranchDto> branches = new List<BranchDto>();

            FirebaseResponse response = await _firebaseDataContext.GetData("Branch");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            foreach (var item in data)
            {
                Branch branch = JsonConvert.DeserializeObject<Branch>(((JProperty)item).Value.ToString());

                TimeSpan timeSpan = branch.LastActive - DateTime.Now;

                float x = (float)(timeSpan.TotalMinutes);

                BranchDto branchDto = new BranchDto()
                {
                    Id = branch.Id,
                    Img = branch.ImgUrl,
                    LastActive = x,
                    Location = branch.Location,
                    Name = branch.Name
                };

                branches.Add(branchDto);
            }

            return branches;
        }
        public async Task<List<Branch>> GetBranches2()//returns a list
        {
            List<Branch> branches = new List<Branch>();

            FirebaseResponse response = await _firebaseDataContext.GetData("Branch");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            if (data != null)
            {
                foreach (var item in data)
                {
                    Branch branch = JsonConvert.DeserializeObject<Branch>(((JProperty)item).Value.ToString());
                    branches.Add(branch);
                }
            }            

            return branches;
        }
    }
}
