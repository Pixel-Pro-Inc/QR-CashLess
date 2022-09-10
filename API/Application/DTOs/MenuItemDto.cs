using System.Collections.Generic;

namespace API.Application.DTOs
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Restuarant { get; set; }
        public float Price { get; set; }
        public string Weight { get; set; }
        public float PrepTime { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ImgUrl { get; set; }
        public float Rate { get; set; }
        public float MinimumPrice { get; set; }
        public bool Availability { get; set; }
        public string publicId { get; set; }
        //New Additions
        public List<string> Flavours { get; set; }
        public List<string> MeatTemperatures { get; set; }
        public List<string> Sauces { get; set; }
    }
}
