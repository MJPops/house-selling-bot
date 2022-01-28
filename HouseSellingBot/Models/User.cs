using System.Collections.Generic;

namespace HouseSellingBot.Models
{
    public class User
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } = "user";
        public string HouseType { get; set; }
        public string HouseMetro { get; set; }
        public string HouseRentType { get; set; }
        public string HouseDistrict { get; set; }
        public int? HouseRoomsNumbe { get; set; }
        public float? LowerPrice { get; set; }
        public float? HightPrice { get; set; }
        public float? LowerFootage { get; set; }
        public float? HightFootage { get; set; }

        public List<House> FavoriteHouses { get; set; } = new();
    }
}
