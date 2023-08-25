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
        private readonly IBlogService _blogService;
        private readonly GenerateQRCode _generateQRCode;

        public ShopController(IProductService productService, IBlogService blogService, GenerateQRCode generateQRCode)
        {
            _productService = productService;
            _blogService = blogService;
            _generateQRCode = generateQRCode;
        }

        public IActionResult Index(int page = 1, int take = 10)
        {
            var paginationVM = _productService.GetProducts(page, take);
            return View(paginationVM);
        }
        public IActionResult Product(int id)
        {
            var exist = _productService.GetProductDetail(id);
            if(exist == null) return NotFound();
            return View(exist);
        }
        public IActionResult GetCategoryProduct(string categoryName)
        {
            var exist = _productService.GetAll().Where(c => c.Category.Name == categoryName).ToList();
            if (exist == null) return NotFound();
            return Json(exist);
        }


        public IActionResult CreateProductComment(int productId, string name, string email, string comment)
        {
            if (name == null || email == null || comment == null) return RedirectToAction(nameof(Product), new { id = productId});
            _blogService.CreateBlogComment(productId, name, email, comment);
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
