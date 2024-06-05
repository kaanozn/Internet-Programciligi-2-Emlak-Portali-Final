using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.UI.Controllers
{
    public class HouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
