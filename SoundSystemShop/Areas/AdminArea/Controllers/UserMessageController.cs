using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Areas.AdminArea.Controllers
{
    [Area(("AdminArea"))]
    public class UserMessageController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly AppDbContext _appDbContext;

        public UserMessageController(IAccountService accountService, AppDbContext appDbContext)
        {
            _accountService = accountService;
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View(_appDbContext.UserMessages.Where(m => m.IsSeen == false).OrderByDescending(m => m.CreationDate).ToList());
        }

        public IActionResult SeenMessage()
        {
            return View(_appDbContext.UserMessages.Where(m => m.IsSeen == true).ToList());
        }
        public IActionResult Seen(int? id)
        {
            if (id == null) return NotFound();
            var message = _appDbContext.UserMessages.FirstOrDefault(x => x.Id == id);
            if (message == null) return NotFound();

            message.IsSeen = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var message = _appDbContext.UserMessages.FirstOrDefault(x => x.Id == id);
            if (message == null) return NotFound();
            if (message.Subject == "Create New Product") return RedirectToAction("Detail", "CustomerProduct", new { id = message.Message, email = message.Email});
            return View(message);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var message = _appDbContext.UserMessages.FirstOrDefault(x => x.Id == id);
            if (message == null) return NotFound();

            _appDbContext.UserMessages.Remove(message);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
