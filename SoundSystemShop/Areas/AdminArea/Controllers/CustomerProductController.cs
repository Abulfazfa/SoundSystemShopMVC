using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Helper;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class CustomerProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _appDbContext;
    private readonly IFileService _fileService;
    private readonly IEmailService _emailService;
    public CustomerProductController(IProductService productService, IUnitOfWork unitOfWork, AppDbContext appDbContext, IFileService fileService, IEmailService emailService)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
        _appDbContext = appDbContext;
        _fileService = fileService;
        _emailService = emailService;
    }

    // GET
    public IActionResult Detail(int id, string email)
    {
        ViewBag.UserEmail = email;
        var cproducts = _appDbContext.Products
            .Include(cp => cp.Images).FirstOrDefault(p => p.Id == id);
        return View(cproducts);
    }
    public IActionResult Create(int id)
    {
        var cproduct = _appDbContext.CustomerProducts
            .Include(cp => cp.ProductImages).FirstOrDefault(cp => cp.Id == id);
        return Content("Created");
    }
    public IActionResult Reject(int id, string email)
    {
        var cproduct = _appDbContext.Products.FirstOrDefault(cp => cp.Id == id);
        cproduct.IsDeleted = false;
        _appDbContext.SaveChanges();
        SendEmailToUser(email, MessageConstants.Reject_UserProduct);
        return RedirectToAction("Index", "Usermessage", new {area = "AdminArea"});
    }
    private void SendEmailToUser(string email, string message)
    {
        string body = string.Empty;
        string path = "verify.html";
        string subject = "Modified New Product";

        EmailMember emailMember = new EmailMember()
        {
            email = email,
            path = path,
            subject = subject,
            saleDesc = "Please check it",
            message = message
        };
        _emailService.PrepareEmail(emailMember);
    }
}