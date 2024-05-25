using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Controllers;

public class HomeController : Controller
{
    private readonly IBannerService _bannerService;
    private readonly IBlogService _blogService;
    private readonly ISliderService _sliderService;
    private readonly UserActivityFilter _userActivityFilter;
    private readonly IProductService _productService;

    public HomeController(IUnitOfWork unitOfWork, IBannerService bannerService, IBlogService blogService, ISliderService sliderService, UserActivityFilter userActivityFilter, IProductService productService, PromoService promo)
    {
        _bannerService = bannerService;
        _blogService = blogService;
        _sliderService = sliderService;
        _userActivityFilter = userActivityFilter;
        _productService = productService;
    }

    public IActionResult Index()
    {
        HomeVW homeVW = new HomeVW();
        homeVW.Sliders = _sliderService.GetAllSliders().Result.ToList();
        homeVW.Banners = _bannerService.GetAllBanners().Result.ToList();
        //homeVW.SocialMedias = _unitOfWork.SocialMediaRepo.GetAll();
        homeVW.Products = _productService.GetAll();
        homeVW.Blogs = _blogService.GetAllBlogs().Result.ToList();
        homeVW.UserActivities = _userActivityFilter.GetRecentlyViewedProducts();
        homeVW.DayOfDiscount = _productService.SaleOfDay();
        return View(homeVW);
    }

    public IActionResult Search(string search)
    {
        var products = _productService.GetAll()
            .Where(p => p.Name.ToLower().Contains(search.ToLower()))
            .Take(3)
            .OrderByDescending(p => p.Id)
            .ToList();
        return PartialView("_SearchPartial", products);
    }
    
}