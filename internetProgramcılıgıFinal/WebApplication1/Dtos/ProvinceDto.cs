namespace WebApplication1.Dtos
{
    public class ProvinceDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<HouseDto> Houses { get; set; }
    }
}
