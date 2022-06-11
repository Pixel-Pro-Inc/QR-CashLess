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

        [Authorize]
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
                Availability = true,
                Flavours = menuItemDto.Flavours,
                MeatTemperatures = menuItemDto.MeatTemperatures,
                Sauces = menuItemDto.Sauces
            };

            if (menuItem.Weight != "0")
                menuItem.Name = String.IsNullOrEmpty(menuItem.Weight) ? menuItem.Name : menuItem.Name + " (" + menuItem.Weight + " grams)";

            List<Subcategory> subCategories = await _firebaseServices.GetData<Subcategory>("SubCategories");

            //You need to comment on why you have this block/logic here, cause it was cool when you showed Mr Billy but ahhh, I couldn't understand why it exists
            if(menuItem.Category == "Meat")
            {
                if (subCategories.Count == 0)
                {
                    List<string> subs = new List<string>();
                    subs.Add(menuItemDto.SubCategory);

                    _firebaseServices.StoreData("SubCategories/" + 0, new Subcategory()
                    {
                        SubCategories = subs
                    });
                }
                else
                {
                    if (!subCategories[0].SubCategories.Contains(menuItemDto.SubCategory))
                    {
                        subCategories[0].SubCategories.Add(menuItemDto.SubCategory);
                        _firebaseServices.StoreData("SubCategories/" + 0, subCategories[0]);
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

            _firebaseServices.StoreData("Menu/" + branchId + "/" + n.ToString(), menuItem);

            return menuItemDto;
        }
        #region Extras
        [HttpGet("subcategory")]
        public async Task<ActionResult<List<string>>> GetSubCategories()
        {
            var response = (await _firebaseServices.GetData<Subcategory>("SubCategories"));

            return response.Count != 0 ? response[0].SubCategories : new List<string>();
        }

        [HttpGet("flavours/create/{flavour}")]
        public async Task<ActionResult<List<string>>> CreateFlavours(string flavour)
        {
            var response = (await _firebaseServices.GetData<Flavour>("Flavours"));

            if (response.Count == 0)
                response.Add(new Flavour());

            response[0].Flavours.Add(flavour);


            _firebaseServices.StoreData("Flavours/" + 0, response[0]);

            return response[0].Flavours;
        }

        [HttpGet("flavours/get")]
        public async Task<ActionResult<List<string>>> GetFlavours()
        {
            var response = (await _firebaseServices.GetData<Flavour>("Flavours"));

            return response.Count != 0 ? response[0].Flavours : new List<string>();
        }

        [HttpGet("meattemperature/create/{meattemperature}")]
        public async Task<ActionResult<List<string>>> CreateMeatTemperature(string meattemperature)
        {
            var response = (await _firebaseServices.GetData<MeatTemperature>("MeatTemperature"));

            if (response.Count == 0)
                response.Add(new MeatTemperature());

            response[0].MeatTemperatures.Add(meattemperature);


            _firebaseServices.StoreData("MeatTemperature/" + 0, response[0]);

            return response[0].MeatTemperatures;
        }

        [HttpGet("meattemperatures/get")]
        public async Task<ActionResult<List<string>>> GetMeatTemperatures()
        {
            var response = (await _firebaseServices.GetData<MeatTemperature>("MeatTemperature"));

            return response.Count != 0 ? response[0].MeatTemperatures : new List<string>();
        }

        [HttpGet("sauce/create/{sauce}")]
        public async Task<ActionResult<List<string>>> CreateSauce(string sauce)
        {
            var response = (await _firebaseServices.GetData<Sauce>("Sauce"));

            if (response.Count == 0)
                response.Add(new Sauce());

            response[0].Sauces.Add(sauce);


            _firebaseServices.StoreData("Sauce/" + 0, response[0]);

            return response[0].Sauces;
        }

        [HttpGet("sauce/get")]
        public async Task<ActionResult<List<string>>> GetSauces()
        {
            var response = (await _firebaseServices.GetData<Sauce>("Sauce"));

            return response.Count != 0 ? response[0].Sauces : new List<string>();
        }
        public async Task<int> GetId(string branchId)
        {
            List<MenuItem> items = await _firebaseServices.GetData<MenuItem>("Menu/" + branchId);

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
        #endregion

        [Authorize]
        [HttpPost("edititem/{id}")]
        public async Task<ActionResult<MenuItemDto>> EditMenuItem(MenuItemDto menuItemDto, string id)
        {
            string branchId = id;

            var menuItems = await _firebaseServices.GetData<MenuItem>("Menu/" + branchId);
            MenuItem oldMenuItem = null;

            foreach (var item in menuItems)
            {
                if(item.Id == menuItemDto.Id)
                {
                    oldMenuItem = item;
                    break;
                }
            }

            #region Assign Values
            oldMenuItem.Name = menuItemDto.Name;
            oldMenuItem.Description = menuItemDto.Description;
            oldMenuItem.prepTime = menuItemDto.PrepTime.ToString();
            oldMenuItem.Price = menuItemDto.Price.ToString("f2");
            oldMenuItem.Weight = menuItemDto.Weight;

            oldMenuItem.Category = menuItemDto.Category;
            oldMenuItem.SubCategory = menuItemDto.SubCategory;

            oldMenuItem.Rate = menuItemDto.Rate;
            oldMenuItem.MinimumPrice = menuItemDto.MinimumPrice;

            oldMenuItem.Availability = true;

            oldMenuItem.Flavours = menuItemDto.Flavours.Count == 0 ? oldMenuItem.Flavours : menuItemDto.Flavours;
            oldMenuItem.MeatTemperatures = menuItemDto.MeatTemperatures.Count == 0 ?
                oldMenuItem.MeatTemperatures : menuItemDto.MeatTemperatures;

            oldMenuItem.Sauces = menuItemDto.Sauces.Count == 0 ? oldMenuItem.Sauces : menuItemDto.Sauces;
            #endregion

            if (oldMenuItem.Weight != "0" && !oldMenuItem.Name.Contains("grams"))
                oldMenuItem.Name = String.IsNullOrEmpty(oldMenuItem.Weight) ? oldMenuItem.Name : oldMenuItem.Name + " (" + oldMenuItem.Weight + " grams)";

            List<Subcategory> subCategories = await _firebaseServices.GetData<Subcategory>("SubCategories");

            //You need to comment on why you have this block/logic here, cause it was cool when you showed Mr Billy but ahhh, I couldn't understand why it exists
            if (oldMenuItem.Category == "Meat")
            {
                if (subCategories.Count == 0)
                {
                    List<string> subs = new List<string>();
                    subs.Add(menuItemDto.SubCategory);

                    _firebaseServices.StoreData("SubCategories/" + 0, new Subcategory()
                    {
                        SubCategories = subs
                    });
                }
                else
                {
                    if (!subCategories[0].SubCategories.Contains(menuItemDto.SubCategory))
                    {
                        subCategories[0].SubCategories.Add(menuItemDto.SubCategory);
                        _firebaseServices.StoreData("SubCategories/" + 0, subCategories[0]);
                    }
                }
            }

            //Detect change in image

            if (!string.IsNullOrEmpty(menuItemDto.ImgUrl))
                if (oldMenuItem.ImgUrl != menuItemDto.ImgUrl)
                {
                    //STORE NEW IMAGE

                    //Delete old image
                    await _photoService.DeletePhotoAsync(oldMenuItem.PublicId);

                    //Upload new image

                    string path = menuItemDto.ImgUrl;

                    if (!string.IsNullOrEmpty(path))
                    {
                        var result = await _photoService.AddPhotoAsync(path);

                        if (result.Error != null) return BadRequest(result.Error.Message);

                        oldMenuItem.ImgUrl = result.SecureUrl.AbsoluteUri;
                        oldMenuItem.PublicId = result.PublicId;
                    }
                }

            _firebaseServices.StoreData("Menu/" + branchId + "/" + oldMenuItem.Id, oldMenuItem);

            return menuItemDto;
        }

        [Authorize]
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