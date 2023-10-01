using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;

namespace SoundSystemShop.Controllers;

public class AboutController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public AboutController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        ViewBag.UserCount = _userManager.Users.Count();
        return View();
    }
}