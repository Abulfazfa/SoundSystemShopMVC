using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Controllers
{
    [Area("AdminArea")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }
        public IActionResult Create()
        {
            return View(_roleManager.Roles.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return BadRequest();
            await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> ChangeRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();
            return View(new RoleChangeVM
            {
                Roles = roles,
                User = user,
                UserRoles = userRoles
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, List<string> newRoles)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, newRoles);
            return Content("Successfully updated");
        }
    }
}

