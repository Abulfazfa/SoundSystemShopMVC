using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Controllers;

public class HomeController : Controller
{
    private readonly IBannerService _bannerService;
    private readonly IBlogService _blogService;
    private readonly ISliderService _sliderService;

    public HomeController(IUnitOfWork unitOfWork, IBannerService bannerService, IBlogService blogService, ISliderService sliderService)
    {
        _bannerService = bannerService;
        _blogService = blogService;
        _sliderService = sliderService;
    }

    public IActionResult Index()
    {
        HomeVW homeVW = new HomeVW();
        homeVW.Sliders = _sliderService.GetAllSliders().Result.ToList();
        homeVW.Banners = _bannerService.GetAllBanners().Result.ToList();
        //homeVW.SocialMedias = _unitOfWork.SocialMediaRepo.GetAll();
        homeVW.Blogs = _blogService.GetAllBlogs().Result.ToList();
        return View(homeVW);
    }

    public IActionResult Search(string search)
    {
        //var products = _appDbContext.Products.Include(p => p.Images).Include(p => p.Category)
        //    .Where(p => p.Name.ToLower().Contains(search.ToLower()))
        //    .Take(3)
        //    .OrderByDescending(p => p.Id)
        //    .ToList();
        //return PartialView("_SearchPartial", products);
        return View();
    }
}