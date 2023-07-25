using Microsoft.AspNetCore.Mvc;
using QRCoder;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace SoundSystemShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenerateQRCode _generateQRCode;

        public ShopController(IUnitOfWork unitOfWork, GenerateQRCode generateQRCode)
        {
            _unitOfWork = unitOfWork;
            _generateQRCode = generateQRCode;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Detail(int id)
        {
            return View(_unitOfWork.ProductRepo.GetByIdAsync(id));
        }

        public IActionResult GetAll(string? categoryName)
        {
            return categoryName == null ? Json(_unitOfWork.BannerRepo.GetAllAsync().Result) : Json(_unitOfWork.BannerRepo.GetByPredicateAsync(b => b.Name == categoryName).Result);
        }

        [HttpPost]
        public ActionResult GenerateQRCode(string json)
        {
            ViewBag.QrCodeUri = _generateQRCode.GenerateQR(json);
            return View();
        }
    }

    
}
