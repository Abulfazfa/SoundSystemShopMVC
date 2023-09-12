using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using System.IO;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area(("AdminArea"))]
public class DashboardController : Controller
{
    private readonly AppDbContext _appDbContext;

    public DashboardController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IActionResult Index()
    {
        var mostPopularProduct = _appDbContext.UserActivities
            .GroupBy(u => u.Url)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();
        int startIndex = mostPopularProduct.LastIndexOf('/') + 1;
        int numberPart = int.Parse(mostPopularProduct.Substring(startIndex));
        var product = _appDbContext.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == numberPart);
        if (mostPopularProduct != null)
        {
            ViewBag.MostPopularProduct = product;
            return View();
        }
        return View();
    }
}