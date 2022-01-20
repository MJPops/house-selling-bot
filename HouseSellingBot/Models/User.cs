using System.Collections.Generic;

namespace HouseSellingBot.Models
{
    public class User
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Name { get; set; }
        public string HouseType { get; set; }
        public string HouseDistrict { get; set; }
        public int? HouseRoomsNumbe { get; set; }
        public float? LowerPrice { get; set; }
        public float? HightPrice { get; set; }
        public float? LowerFootage { get; set; }
        public float? HightFootage { get; set; }

        public int? HouseId { get; set; }
        public List<House> FavoriteHouses { get; set; } = new();
    }
}
