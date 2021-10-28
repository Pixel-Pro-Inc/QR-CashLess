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

namespace API.Controllers
{
    public class MenuController : BaseApiController
    {
        private DataContext _context;
        private IPhotoService _photoService;

        public MenuController(DataContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        [HttpGet("getmenu")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu()
        {
            return await _context.MenuItems.ToListAsync() == null? new List<MenuItem>(): await _context.MenuItems.ToListAsync();
        }
        [HttpPost("createitem")]
        public async Task<ActionResult<MenuItemDto>> AddMenuItem(MenuItemDto menuItemDto)
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

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItemDto;
        }
    }
}