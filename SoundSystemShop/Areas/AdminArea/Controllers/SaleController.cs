using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Services;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IProductService _productService;

        public SaleController(ISaleService saleService, IProductService productService)
        {
            _saleService = saleService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View(_saleService.GetAll());
        }
        public IActionResult Create()
        {
            ViewBag.Products = _productService.GetProductSelectList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(SaleVM saleVM)
        {
            ViewBag.Products = _productService.GetProductSelectList();
            if (ModelState.IsValid)
            {
                _saleService.Create(saleVM);
                return RedirectToAction("Index");
            }
            
            return View();
        }
        
        public IActionResult Delete(int id)
        {
            _saleService.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            ViewBag.Products = _productService.GetProductSelectList();
            var saleVM = _saleService.MapSale(id);
            if (saleVM == null) return NotFound();
            return View(saleVM);
        }
        [HttpPost]
        public IActionResult Update(int id, SaleVM saleVM)
        {
            ViewBag.Products = _productService.GetProductSelectList();
            if (_saleService.Update(id, saleVM)) return RedirectToAction("Index");
            return View(saleVM);
        }
        public IActionResult ABCD()
        {
            _saleService.SendSaleEmail();
            return Json("Sended");
        }


    }
}
