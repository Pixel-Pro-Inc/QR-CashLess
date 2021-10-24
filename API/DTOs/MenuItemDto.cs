﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class MenuItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Restuarant { get; set; }
        public float Price { get; set; }
        public float PrepTime { get; set; }
        public string Category { get; set; }
        public string ImgUrl { get; set; }
    }
}