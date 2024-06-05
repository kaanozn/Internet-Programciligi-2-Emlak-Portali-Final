using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.UI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
