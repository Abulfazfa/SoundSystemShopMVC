using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Controllers
{
    public class OwnProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OwnProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult CreateProduct()
        {
            return View();
        }
    }
}
