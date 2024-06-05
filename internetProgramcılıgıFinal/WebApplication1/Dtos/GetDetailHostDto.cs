using WebApplication1.Models;

namespace WebApplication1.Dtos
{
    public class GetDetailHostDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int RoomCount { get; set; }
        public int FloorCount { get; set; }
        public int Area { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public HouseImagesDto HouseImages { get; set; }
        public DistrictAddDto District { get; set; }
        public ProvinceAddDto Province { get; set; }
        public AppUserDto AppUser { get; set; }
    }
}
