using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult GetAll(string? name)
        {
            return name == null ? Json(_unitOfWork.BannerRepo.GetAll()) : Json(_unitOfWork.BannerRepo.Get(b => b.Name == name));
        }
    }
}
