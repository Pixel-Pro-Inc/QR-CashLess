using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Restuarant { get; set; }
        public float Price { get; set; }
        public float PrepTime { get; set; }
        public string Category { get; set; }
        public string ImgUrl { get; set; }
        public float Rate { get; set; }
        public float MinimumPrice { get; set; }
        public bool Availability { get; set; }
        public string publicId { get; set; }
    }
}
