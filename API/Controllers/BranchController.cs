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
        public BranchController(IPhotoService photoService, IFirebaseServices firebaseServices):base(firebaseServices)
        {
            _photoService = photoService;
        }

        /// <summary>
        /// Takes in a <see cref="BranchDto"/> from the client so that the controller method have something to work with.
        /// <para> Then creates a branch if it meets the criteria and stores it in the database</para>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<BranchDto>> CreateBranch(BranchDto dto)
        {
            List<Branch> branches = new List<Branch>();
            branches = await _firebaseServices.GetBranchesFromDatabase();

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

            _firebaseServices.StoreData("Branch/" + dto.Id, branch);

            return dto;
        }

        /// <summary>
        /// This is called from the client to get all the branches in the database
        /// 
        /// <para> The branches will come in as <see cref="BranchDto"/> instead of just the branch types</para>
        /// <para> NOTE: The branchDtos are made within the method</para>
        /// </summary>
        /// <returns> IEnumerable <see cref="BranchDto"/></returns>
        [HttpGet("getbranches")]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetBranches()
        {
            List<BranchDto> branches = new List<BranchDto>();

            var response = await _firebaseServices.GetBranchesFromDatabase();

            foreach (var item in response)
            {
                TimeSpan timeSpan = DateTime.UtcNow - item.LastActive;

                float x = (float)(timeSpan.TotalMinutes);

                BranchDto branchDto = new BranchDto()
                {
                    Id = item.Id,
                    Img = item.ImgUrl,
                    LastActive = x,
                    Location = item.Location,
                    Name = item.Name,
                    PhoneNumber = item.PhoneNumber.ToString()
                };

                branches.Add(branchDto);
            }

            return branches;
        }
        
    }
}
