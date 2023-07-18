using Microsoft.AspNetCore.Mvc;

namespace SoundSystemShop.Areas.AdminArea.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Register()
    {
        return View();
    }
    public IActionResult Login()
    {
        return View();
    }
}