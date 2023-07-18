using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Services;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Controllers;

public class HomeController : Controller
{
    private readonly SliderRepository _sliderRepository;
    private readonly BlogRepository _blogRepository;

    public HomeController(SliderRepository sliderRepository, BlogRepository blogRepository)
    {
        _sliderRepository = sliderRepository;
        _blogRepository = blogRepository;
    }

    public IActionResult Index()
    {
        HomeVW homeVW = new HomeVW();
        homeVW.Sliders = _sliderRepository.GetAll();
        homeVW.Blogs = _blogRepository.GetAll();
        //homeVW.SocialMedias = _socialMediaRepository.GetAll();
        return View(homeVW);
    }

    // public IActionResult Search()
    // {
    //     return View();
    // }
}