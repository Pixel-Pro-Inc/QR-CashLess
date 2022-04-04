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

        public MenuController(IPhotoService photoService):base()
        {
            _photoService = photoService;
        }

        [HttpGet("getmenu/{branchId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu(string branchId)
        {
            return await _firebaseDataContext.GetData<MenuItem>("Menu/" + branchId);
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
                Weight = menuItemDto.Weight,
                Restuarant = menuItemDto.Restuarant,
                Category = menuItemDto.Category,
                SubCategory = menuItemDto.SubCategory,
                Rate = menuItemDto.Rate,
                MinimumPrice = menuItemDto.MinimumPrice,
                Availability = true
            };

            menuItem.Name = String.IsNullOrEmpty(menuItem.Weight)? menuItem.Name: menuItem.Name + " (" + menuItem.Weight + " grams)";

            List<Subcategory> subCategories = await _firebaseDataContext.GetData<Subcategory>("SubCategories");

            if(menuItem.Category == "Meat")
            {
                if (subCategories.Count == 0)
                {
                    List<string> subs = new List<string>();
                    subs.Add(menuItemDto.SubCategory);

                    _firebaseDataContext.StoreData("SubCategories/" + 0, new Subcategory()
                    {
                        SubCategories = subs
                    });
                }
                else
                {
                    if (!subCategories[0].SubCategories.Contains(menuItemDto.SubCategory))
                    {
                        subCategories[0].SubCategories.Add(menuItemDto.SubCategory);
                        _firebaseDataContext.StoreData("SubCategories/" + 0, subCategories[0]);
                    }
                }
            }

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

            _firebaseDataContext.StoreData("Menu/" + branchId + "/" + n.ToString(), menuItem);

            return menuItemDto;
        }
        [HttpGet("subcategory")]
        public async Task<ActionResult<List<string>>> GetSubCategories()
        {
            var response = (await _firebaseDataContext.GetData<Subcategory>("SubCategories"));

            return response.Count != 0? response[0].SubCategories: new List<string>();
        }
        public async Task<int> GetId(string branchId)
        {
            List<MenuItem> items = await _firebaseDataContext.GetData<MenuItem>("Menu/" + branchId);

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
            List<MenuItem> items = await _firebaseDataContext.GetData<MenuItem>("Menu/" + branchId);

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

            //Update firebase

            _firebaseDataContext.EditData("Menu/" + branchId + "/" + menuItem.Id, menuItem);

            return menuItemDto;
        }

        [HttpPost("delete/{branchId}")]
        public async Task<ActionResult<MenuItemDto>> DeleteMenuItem(MenuItemDto menuItemDto, string branchId)
        {
            //Delete Image from Cloudinary
            await _photoService.DeletePhotoAsync(menuItemDto.publicId);

            //Delete Item from Firebase
            _firebaseDataContext.DeleteData("Menu/" + branchId + "/" + menuItemDto.Id);

            return menuItemDto;
        }
    }
}