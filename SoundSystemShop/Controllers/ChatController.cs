using Microsoft.AspNetCore.Mvc;

namespace SoundSystemShop.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
