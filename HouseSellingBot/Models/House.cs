using System.Collections.Generic;

namespace HouseSellingBot.Models
{
    public class House
    {
        public int Id { get; set; }
        public string WebPath { get; set; }
        public string PicturePath { get; set; }
        public string Description { get; set; }
        public string District { get; set; }
        public string Type { get; set; }
        public string Metro { get; set; }
        public string RentType { get; set; }
        public int? RoomsNumber { get; set; }
        public float? Price { get; set; }
        public float? Footage { get; set; }

        public int? UserId { get; set; }
        public List<User> Users { get; set; } = new();
    }
}
