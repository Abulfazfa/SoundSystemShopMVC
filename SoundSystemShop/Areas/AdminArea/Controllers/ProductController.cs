using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public IActionResult Index(int page = 1, int take = 2)
    {
        var paginationVM = _productService.GetProducts(page, take);
        if (paginationVM.Items.Count > 0)
        {
            return View(paginationVM);
        }
        else
        {
            return RedirectToAction("Create");
        }
    }

    public IActionResult Detail(int id)
    {
        var product = _productService.GetProductDetail(id);
        return View(product);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = _categoryService.GetCategorySelectList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductVM productVM)
    {
        ViewBag.Categories = _categoryService.GetCategorySelectList();
        if (!ModelState.IsValid)
            return Content("IsValid");

        if (_productService.CreateProduct(productVM).Result)
            return RedirectToAction("Index");
        else
            return View();
    }

    public IActionResult Delete(int id)
    {
        if (_productService.DeleteProduct(id).Result)
            return RedirectToAction(nameof(Index));
        return BadRequest();
    }

    public IActionResult Update(int id)
    {
        ViewBag.Id = id;
        var product = _productService.GetProductDetail(id);
        if (product == null)
            return NotFound();

        var productVM = _productService.MapProductVMAndProduct(id);

        ViewBag.Categories = _categoryService.GetCategorySelectList();
        return View(productVM);
    }

    [HttpPost]
    public IActionResult Update(int id, ProductVM productVM)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Update), productVM);

        if (_productService.UpdateProduct(id, productVM))
            return RedirectToAction(nameof(Index));
        else
            return NotFound();
    }

    public IActionResult DeleteImage(string imgUrl, int id)
    {
        if (_productService.DeleteProductImage(imgUrl, id))
            return RedirectToAction(nameof(Update), new { Id = id });
        else
            return NotFound();
    }
}
