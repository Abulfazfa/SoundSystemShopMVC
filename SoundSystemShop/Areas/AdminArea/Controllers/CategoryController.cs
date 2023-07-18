using Microsoft.AspNetCore.Mvc;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class CategoryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}