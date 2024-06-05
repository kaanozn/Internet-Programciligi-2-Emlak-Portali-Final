using WebApplication1.Models;

namespace WebApplication1.Dtos
{
    public class DistrictDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public List<HouseDto> Houses { get; set; }
    }
}
