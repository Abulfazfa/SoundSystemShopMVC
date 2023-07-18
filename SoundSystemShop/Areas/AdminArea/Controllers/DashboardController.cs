using Microsoft.AspNetCore.Mvc;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area(("AdminArea"))]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}