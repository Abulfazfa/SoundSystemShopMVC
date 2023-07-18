using Microsoft.AspNetCore.Mvc;

namespace SoundSystemShop.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            //return View();
            return Content("/Shop");
        }
    }
}
