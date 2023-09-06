using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;

public class CustomerProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _appDbContext;
    public CustomerProductController(IProductService productService, IUnitOfWork unitOfWork, AppDbContext appDbContext)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
        _appDbContext = appDbContext;
    }

    // GET
    public IActionResult Index()
    {
        var cproducts = _appDbContext.CustomerProducts
            .Include(cp => cp.ProductImages).Where(cp => cp.IsDeleted == true).ToList();
        return View();
    }
    public IActionResult Create(int id)
    {
        var cproduct = _appDbContext.CustomerProducts
            .Include(cp => cp.ProductImages).FirstOrDefault(cp => cp.Id == id);
        return View();
    }
    public IActionResult Reject(int id)
    {
        var cproduct = _appDbContext.CustomerProducts.FirstOrDefault(cp => cp.Id == id);
        cproduct.IsDeleted = false;
        _appDbContext.SaveChanges();
        return NoContent();
    }
}