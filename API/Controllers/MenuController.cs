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
        private IPhotoService _photoService;

        public MenuController(IPhotoService photoService, IFirebaseServices firebaseServices):base(firebaseServices)
        {
            _photoService = photoService;
        }

        [HttpGet("getmenu/{branchId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu(string branchId)
        {
            List<MenuItem> items = await _firebaseServices.GetMenu(branchId);

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

            menuItem.Id = n;

            _firebaseServices.StoreData("Menu/" + branchId + "/" + n.ToString(), menuItem);

            return menuItemDto;
        }
        public async Task<int> GetId(string branchId)
        {
            List<MenuItem> items = await _firebaseServices.GetMenu(branchId);

            if (items.Count != 0)
            {

                List<int> contents = new List<int>();

                for (int i = 0; i < items.Count; i++)
                {
                    contents.Add(items[i].Id);
                }

                int k = 0;

                while (contents.Contains(k))
                {
                    k++;
                }

                return k;
            }

            return 0;            
        }
        [HttpPost("edititem/{id}")]
        public async Task<ActionResult<MenuItemDto>> EditMenuItem(MenuItemDto menuItemDto, string id)
        {
            string branchId = id;

            var menuItem = new MenuItem
            {
                Id = menuItemDto.Id,
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                prepTime = menuItemDto.PrepTime.ToString(),
                Price = menuItemDto.Price.ToString("f2"),
                Restuarant = menuItemDto.Restuarant,
                Category = menuItemDto.Category,
                Rate = menuItemDto.Rate,
                MinimumPrice = menuItemDto.MinimumPrice,
                Availability = true,
                ImgUrl = menuItemDto.ImgUrl,
                PublicId = menuItemDto.publicId
            };

            //Detect change in image
            List<MenuItem> items = new List<MenuItem>();

            var response = await _firebaseServices.GetMenu( branchId);

            var mItem = items.Where(i => i.Id == menuItem.Id);

            MenuItem temp = null;

            foreach (var m in mItem)
            {
                temp = m;
            }

            if(temp.ImgUrl != menuItemDto.ImgUrl)
            {
                //STORE NEW IMAGE

                //Delete old image
                await _photoService.DeletePhotoAsync(menuItem.PublicId);

                //Upload new image

                string path = menuItemDto.ImgUrl;

                if (path != null && path != "")
                {
                    var result = await _photoService.AddPhotoAsync(path);

                    if (result.Error != null) return BadRequest(result.Error.Message);

                    menuItem.ImgUrl = result.SecureUrl.AbsoluteUri;
                    menuItem.PublicId = result.PublicId;
                }
            }            

            //Updates firebase
            // UPDATE: I noticed that StoreData and edit have the same function so I reuse them
            _firebaseServices.StoreData("Menu/" + branchId + "/" + menuItem.Id, menuItem);

            return menuItemDto;
        }

        [HttpPost("delete/{branchId}")]
        public async Task<ActionResult<MenuItemDto>> DeleteMenuItem(MenuItemDto menuItemDto, string branchId)
        {
            //Delete Image from Cloudinary
            await _photoService.DeletePhotoAsync(menuItemDto.publicId);

            //Delete Item from Firebase
            _firebaseServices.DeleteData("Menu/" + branchId + "/" + menuItemDto.Id);

            return menuItemDto;
        }
    }
}