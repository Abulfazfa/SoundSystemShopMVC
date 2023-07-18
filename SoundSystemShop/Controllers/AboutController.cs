using Microsoft.AspNetCore.Mvc;

namespace SoundSystemShop.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}