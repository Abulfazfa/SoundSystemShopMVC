using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Helper;
using SoundSystemShop.Services;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        var categories = _categoryService.GetCategories().Result;
        return View(categories);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = _categoryService.GetCategorySelectList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(CategoryVM categoryVM)
    {
        ViewBag.Categories = _categoryService.GetCategorySelectList();
        if (!ModelState.IsValid) return View();

        if (categoryVM.Photo != null && !categoryVM.Photo.CheckFileType())
        {
            ModelState.AddModelError("Photo", "Select an image");
            return View();
        }

        _categoryService.AddCategory(categoryVM);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        if (id == null) return NotFound();

        if(_categoryService.DeleteCategory(id).Result) return RedirectToAction("Index");
        return NotFound();
        
    }

    public IActionResult Update(int id)
    {
        var exist = _categoryService.GetCategoryById(id);
        ViewBag.Categories = _categoryService.GetCategorySelectList(id);
        CategoryVM categoryVM = _categoryService.MapCategoryVMAndCategory(id);

        return View(categoryVM);
    }

    [HttpPost]
    public IActionResult Update(int id, CategoryVM categoryVM)
    {
        ViewBag.Categories = _categoryService.GetCategorySelectList(id);
        if (!ModelState.IsValid) return View();

        _categoryService.UpdateCategory(id, categoryVM);
        return RedirectToAction("Index");
    }
}
