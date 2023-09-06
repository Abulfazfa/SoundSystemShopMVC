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
        private readonly IProductService _productService;
        private readonly GenerateQRCode _generateQRCode;
        private readonly IUnitOfWork _unitOfWork;

        public ShopController(IProductService productService, GenerateQRCode generateQRCode, IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _generateQRCode = generateQRCode;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int page = 1, int take = 10)
        {
            ViewBag.ShopProductCategory = _unitOfWork.CategoryRepo.GetAllAsync().Result;
            var paginationVM = _productService.GetProducts(page, take);
            return View(paginationVM);
        }
        public IActionResult Product(int id)
        {
            var exist = _productService.GetProductDetail(id);
            ViewBag.RelatedProducts = _productService.GetAll().Where(p => p.CategoryId == exist.CategoryId).ToList();
            if(exist == null) return NotFound();
            return View(exist);
        }
        public IActionResult GetCategoryProduct(string categoryName)
        {
            var exist = _productService.GetAll().Where(c => c.Category.Name == categoryName).ToList();
            if (exist == null) return NotFound();
            return Json(exist);
        }
        public IActionResult ShopOrderPrice(string str)
        {
            var exist = str == "htl" ? _productService.GetAll().OrderByDescending(p => p.Price).ToList() : _productService.GetAll().OrderBy(p => p.Price).ToList();
            if (exist == null) return NotFound();
            return Json(exist);
        }
        public IActionResult CreateProductComment(int productId, string name, string? email, string comment)
        {
            if (name == null || email == null || comment == null) return RedirectToAction(nameof(Product), new { id = productId});
            _productService.CreateProductComment(productId, name, email, comment);
            return RedirectToAction(nameof(Product), new { id = productId });
        }
        [HttpGet]
        public ActionResult GenerateQRCode()
        {
            //ViewBag.QrCodeUri = _generateQRCode.GenerateQR(json);
            return View();
        }
        [HttpPost]
        public ActionResult GenerateQRCode(string json)
        {
            ViewBag.QrCodeUri = _generateQRCode.GenerateQR(json);
            return View();
        }
        public ActionResult FinishDateOfSale(string name)
        {
            var finishDate = _productService.FinishDateOfSale(name);
            return Json(finishDate);
        }
    }
}
