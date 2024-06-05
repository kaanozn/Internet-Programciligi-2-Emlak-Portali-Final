using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        

        public List<House> House { get; set; }
    }
}
