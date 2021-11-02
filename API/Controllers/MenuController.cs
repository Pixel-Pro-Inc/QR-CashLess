using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    public class MenuController : BaseApiController
    {
        private DataContext _context;
        private IPhotoService _photoService;
        private readonly FirebaseDataContext _firebaseDataContext;

        public MenuController(DataContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
            _firebaseDataContext = new FirebaseDataContext();
        }

        [HttpGet("getmenu/{branchId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu(string branchId)
        {
            List<MenuItem> items = new List<MenuItem>();

            var response = await _firebaseDataContext.GetData("Menu/" + branchId);

            for (int i = 0; i < response.Count; i++)
            {
                MenuItem item = JsonConvert.DeserializeObject<MenuItem>(((JObject)response[i]).ToString());
                items.Add(item);
            }

            return items;
        }

        [HttpPost("createitem/{id}")]
        public async Task<ActionResult<MenuItemDto>> AddMenuItem(MenuItemDto menuItemDto, string id)
        {
            string branchId = id;

            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                prepTime = menuItemDto.PrepTime.ToString(),
                Price = menuItemDto.Price.ToString("f2"),
                Restuarant = menuItemDto.Restuarant,
                Category = menuItemDto.Category,
                Rate = menuItemDto.Rate,
                MinimumPrice = menuItemDto.MinimumPrice,
                Availability = true
            };

            string path = menuItemDto.ImgUrl;

            if (path != null && path != "")
            {
                var result = await _photoService.AddPhotoAsync(path);

                if (result.Error != null) return BadRequest(result.Error.Message);

                menuItem.ImgUrl = result.SecureUrl.AbsoluteUri;
                menuItem.PublicId = result.PublicId;
            }

            int n = await GetId(branchId);

            _firebaseDataContext.StoreData("Menu/" + branchId + "/" + n.ToString(), menuItem);

            return menuItemDto;
        }
        public async Task<int> GetId(string branchId)
        {
            List<MenuItem> items = new List<MenuItem>();

            var response = await _firebaseDataContext.GetData("Menu/" + branchId);

            foreach (var item in response)
            {
                MenuItem data = JsonConvert.DeserializeObject<MenuItem>(((JObject)item).ToString());
                items.Add(data);
            }

            if (items.Count != 0)
            {
                return items[items.Count - 1].Id + 1;
            }

            return 0;            
        }
    }
}