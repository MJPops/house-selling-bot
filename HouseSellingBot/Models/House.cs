using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseSellingBot.Models
{
    public class House
    {
        public string Id { get; set; }
        public string PicturePath { get; set; }
        public string Description { get; set; }
        public string District { get; set; }
        public string HouseType { get; set; }
        public string RentType { get; set; }
        public int RoomsNumber { get; set; }
        public float Price { get; set; }
        public float Footage { get; set; }
    }
}
