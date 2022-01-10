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
        private IPhotoService _photoService;
        public BranchController(IPhotoService photoService):base()
        {
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
                dto.Id = item.Id;

                while (item.Id == dto.Id)
                {
                    dto.Id = "rd" + new Random().Next(10000, 100000);
                    i = branches.Count + 1;
                }
            }

            Branch branch = new Branch()
            {
                Id = dto.Id,
                Location = dto.Location,
                Name = dto.Name,
                PhoneNumber = Int32.Parse(dto.PhoneNumber)
            };

            string path = dto.Img;

            if (path != null && path != "")
            {
                var result = await _photoService.AddPhotoAsync(path);

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

            var response = await _firebaseDataContext.GetData("Branch");

            foreach (var item in response)
            {
                Branch branch = JsonConvert.DeserializeObject<Branch>(((JObject)item).ToString());

                TimeSpan timeSpan = DateTime.UtcNow - branch.LastActive;

                float x = (float)(timeSpan.TotalMinutes);

                BranchDto branchDto = new BranchDto()
                {
                    Id = branch.Id,
                    Img = branch.ImgUrl,
                    LastActive = x,
                    Location = branch.Location,
                    Name = branch.Name,
                    PhoneNumber = branch.PhoneNumber.ToString()
                };

                branches.Add(branchDto);
            }

            return branches;
        }
        public async Task<List<Branch>> GetBranches2()
        {
            List<Branch> branches = new List<Branch>();

            var response = await _firebaseDataContext.GetData("Branch");

            foreach (var item in response)
            {
                Branch branch = JsonConvert.DeserializeObject<Branch>(((JObject)item).ToString());
                branches.Add(branch);
            }

            return branches;
        }
    }
}
