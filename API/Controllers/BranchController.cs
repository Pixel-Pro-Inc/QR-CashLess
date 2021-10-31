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

            foreach (var item in await GetBranches2())
            {
                branches.Add(item);
            }

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
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranches()
        {
            List<Branch> branches = new List<Branch>();

            FirebaseResponse response = await _firebaseDataContext.GetData("Branch");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            foreach (var item in data)
            {
                Branch branch = JsonConvert.DeserializeObject<Branch>(((JObject)item).ToString());
                branches.Add(branch);
            }

            return branches;
        }

        public async Task<IEnumerable<Branch>> GetBranches2()
        {
            List<Branch> branches = new List<Branch>();

            FirebaseResponse response = await _firebaseDataContext.GetData("Branch");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            foreach (var item in data)
            {
                Branch branch = JsonConvert.DeserializeObject<Branch>(((JObject)item).ToString());
                branches.Add(branch);
            }

            return branches;
        }
    }
}
