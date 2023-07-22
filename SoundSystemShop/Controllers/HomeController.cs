using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        HomeVW homeVW = new HomeVW();
        homeVW.Sliders = _unitOfWork.SliderRepo.GetAll();
        homeVW.Banners = _unitOfWork.BannerRepo.GetAll();
        homeVW.SocialMedias = _unitOfWork.SocialMediaRepo.GetAll();
        homeVW.Blogs = _unitOfWork.BlogRepo.GetAll();
        return View(homeVW);
    }

    // public IActionResult Search()
    // {
    //     return View();
    // }
}