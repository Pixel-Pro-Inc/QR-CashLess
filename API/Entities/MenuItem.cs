using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Restuarant { get; set; }
        public float Price { get; set; }
        public string ImgUrl { get; set; }
        public string PublicId { get; set; }
        public string prepTime { get; set; }
        public string Category { get; set; }
    }
}