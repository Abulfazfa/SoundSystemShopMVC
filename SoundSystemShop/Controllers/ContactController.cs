using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Migrations;
using SoundSystemShop.Models;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public ContactController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(ContactVM contactVM)
        {
            if(!ModelState.IsValid) return View(contactVM);
            UserMessage userMessage = new()
            {
                UserName = contactVM.UserName,
                Email = contactVM.Email,
                Message = contactVM.Message,
                Subject = "Contact with me"
            };
            _appDbContext.UserMessages.Add(userMessage);
            _appDbContext.SaveChanges();
            return Content("Send successfully");
        }
    }
}