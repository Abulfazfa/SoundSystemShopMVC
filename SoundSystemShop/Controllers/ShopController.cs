using Microsoft.AspNetCore.Mvc;
using QRCoder;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;
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

        public IActionResult Index(int page = 1, int take = 18)
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
        public IActionResult CreateProductComment(int productId, string comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                if (comment == null)
                {
                    return RedirectToAction(nameof(Product), new { id = productId });
                }
                _productService.CreateProductComment(productId, userName, comment);
                return RedirectToAction(nameof(Product), new { id = productId });
            }
            else
            {
                string returnUrl = Url.Action("CreateProductComment", "Shop", new { productId = productId, comment = comment });

                return RedirectToAction("Login", "Account", new { ReturnUrl = returnUrl });
            }
        }
        public IActionResult DeleteProductComment(int blogId, int commentId)
        {
            _productService.DeleteComment(blogId, commentId);
            return RedirectToAction(nameof(Product), new { id = blogId });
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

        public IActionResult FilterProducts(string str = "htl", int categoryId = 0, int min = 0, int max = 0)
        {
            var products = _productService.GetAll().AsQueryable();
            if (categoryId != 0) { products = products.Where(p => p.CategoryId == categoryId); }

            if (min != 0 && max != 0) { products = products.Where(p => p.Price >= min && p.Price <= max); }

            var exist = str == "htl" ? products.OrderByDescending(p => p.Price).ToList() : products.OrderBy(p => p.Price).ToList();
            ViewBag.SelectedCategoryId = categoryId;
            return PartialView("_ProductListPartial", exist);
        }
    }
}
