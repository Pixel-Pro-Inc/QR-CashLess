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

        [HttpGet("getmenu")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu(string branchID)
        {
            List<MenuItem> items = new List<MenuItem>();

            var response = await _firebaseDataContext.GetData("Menu/" + branchID);

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            
            foreach (var item in data)
            {
                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(((JObject)item).ToString());
                items.Add(menuItem);
            }

            return items;
        }
        [HttpPost("createitem")]
        public async Task<ActionResult<MenuItemDto>> AddMenuItem(MenuItemDto menuItemDto, string branchID)
        {
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

                menuItem.ImgUrl = result.SecureUrl.AbsoluteUri;
                menuItem.PublicId = result.PublicId;
            }

            branchID = "rd543211";

            _firebaseDataContext.StoreData("Menu/" + branchID + "/" + GetId(branchID), menuItem);

            return menuItemDto;
        }
        public async Task<int> GetId(string branchID)
        {
            List<MenuItem> items = new List<MenuItem>();

            var response = await _firebaseDataContext.GetData("Menu/" + branchID);

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            branchID = "rd543211";

            foreach (var item in data)
            {
                MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(((JObject)item).ToString());
                items.Add(menuItem);
            }           

            if(items.Count != 0)
            {
                return items[items.Count - 1].Id + 1;
            }

            return 0;
            
        }
    }
}