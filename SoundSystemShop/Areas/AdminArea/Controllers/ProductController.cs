using Microsoft.AspNetCore.Mvc;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}