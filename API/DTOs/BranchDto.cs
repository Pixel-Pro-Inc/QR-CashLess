using API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class BranchDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Img { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public float LastActive { get; set; }
        public object OpeningTime { get; set; }
        public object ClosingTime { get; set; }
        public object OpeningTimeTomorrow { get; set; }
    }
}
