using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class BannerController : Controller
{
    private readonly IBannerService _bannerService;
    public BannerController(IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    public IActionResult Index()
    {
        var banners = _bannerService.GetAllBanners().Result.ToList();
        return View(banners);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Create(BannerVM bannerVM)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        try
        {
            _bannerService.CreateBanner(bannerVM);
            return RedirectToAction("Index");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("Photo", ex.Message);
            return View();
        }
    }
    public IActionResult Delete(int id)
    {
        var result = _bannerService.DeleteBanner(id).Result;
        if (!result)
        {
            ModelState.AddModelError("", "Happen some problems");
            return View();
        }
        return RedirectToAction("Index");
    }
    public IActionResult Detail(int id)
    {
        return View(_bannerService.GetBannerById(id));
    }
    public IActionResult Update(int id)
    {
        var bannerVM = _bannerService.MapBannerVMAndBanner(id);
        if (bannerVM == null)
        {
            ModelState.AddModelError("", "Happen problem");
            return View();
        }
        return View(bannerVM);
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Update(int id, BannerVM bannerVM)
    {
        if(_bannerService.UpdateBanner(id, bannerVM).Result) return RedirectToAction("Index");
        return View(bannerVM);

    }
}