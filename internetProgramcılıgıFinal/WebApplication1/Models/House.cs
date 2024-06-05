namespace WebApplication1.Models
{
    public class House
    {
        public int Id { get; set; }
        public int HouseImagesId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public string AppUserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int RoomCount { get; set; }
        public int FloorCount { get; set; }
        public int Area { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        
        public HouseImages HouseImages { get; set; }
        public District District { get; set; }
        public Province Province { get; set; }
        public AppUser AppUser { get; set; }
  
    }
}
