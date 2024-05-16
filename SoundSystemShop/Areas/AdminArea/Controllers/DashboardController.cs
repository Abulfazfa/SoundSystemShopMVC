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
     //   var mostPopularProduct = _appDbContext.UserActivities
     //.GroupBy(u => u.Url)
     //.OrderByDescending(g => g.Count())
     //.Select(g => new { Url = g.Key, ClickCount = g.Count() })
     //.FirstOrDefault();

     //   if (mostPopularProduct != null)
     //   {
     //       int startIndex = mostPopularProduct.Url.LastIndexOf('/') + 1;
     //       int numberPart = int.Parse(mostPopularProduct.Url.Substring(startIndex));
     //       var product = _appDbContext.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == numberPart);

     //       ViewBag.MostPopularProduct = product;
     //       ViewBag.MostPopularProductClickCount = mostPopularProduct.ClickCount;
     //   }

        return View();
    }
    public IActionResult GoToMainWebsite()
    {
        // Generate the URL to the main website's Home/Index action
        var url = Url.Action("Index", "Home", new { area = "" });

        // Redirect to the main website
        return Redirect(url);
    }

}