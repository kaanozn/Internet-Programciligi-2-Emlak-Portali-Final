using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.UI.Controllers
{
    public class DistrictController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
