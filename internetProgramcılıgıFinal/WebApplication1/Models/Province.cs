namespace WebApplication1.Models
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<House> Houses { get; set; }
    }
}
