using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;
using static QRCoder.PayloadGenerator;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IFileService _fileService;
    private readonly IEmailService _emailService;

    public ProductController(IProductService productService, ICategoryService categoryService, IFileService fileService, IEmailService emailService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _fileService = fileService;
        _emailService = emailService;
    }

    public IActionResult Index(int page = 1, int take = 10)
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
    public IActionResult Modify(string id, string email)
    {
        SendEmailToUser(email, "Your product successfully modified");
        var productId = int.Parse(id);
        return RedirectToAction(nameof(Update), new { id = productId });
    }
    private void SendEmailToUser(string email, string message)
    {
        string body = string.Empty;
        string path = "verify.html";
        string subject = "Modified New Product";

        EmailMember emailMember = new EmailMember();
        emailMember.email = email;
        emailMember.path = path;
        emailMember.subject = subject;
        emailMember.saleDesc = "Please check it";
        emailMember.message = message;
        _emailService.PrepareEmail(emailMember);
    }
}
