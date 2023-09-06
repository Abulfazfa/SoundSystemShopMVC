using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IAccountService accountService, UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        public IActionResult Index(string name)
        {
            var users = name != null ? _userManager.Users.Where(u => u.Fullname.ToLower().Contains(name.ToLower())).ToList() :
                 _userManager.Users.Where(u => u.UserName != "Admin").ToList();
            return View(users);
        }
        public async Task<IActionResult> BlockOrActive(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}
