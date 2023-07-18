using Microsoft.AspNetCore.Mvc;

namespace SoundSystemShop.Areas.AdminArea.Controllers;

public class BlogController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}