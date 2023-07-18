using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class BannerController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUnitOfWork _unitOfWork;

    public BannerController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
    {
        _webHostEnvironment = webHostEnvironment;
        _unitOfWork = unitOfWork;
    }



    public IActionResult Index()
    {
        var banner = _unitOfWork.BannerRepo.GetAll();
        return View(banner);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(BannerVM bannerVM)
    {
        if (!bannerVM.Photo.CheckFileType())
        {
            ModelState.AddModelError("Photo", "Sellect a image");
            return View();

        }

        Banner banner = new()
        {
            ImgUrl = bannerVM.Photo.SaveImage(_webHostEnvironment, "assets/img/banner"),
        };
        banner.Name = bannerVM.Name;
        _unitOfWork.BannerRepo.Add(banner);
        _unitOfWork.Commit();

        return RedirectToAction("Index");
    }
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();
        var banner = _unitOfWork.BannerRepo.Get(x => x.Id == id);
        if (banner == null) return NotFound();

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/banner", banner.ImgUrl);
        DeleteHelper.DeleteFile(path);
        _unitOfWork.BannerRepo.Delete(x => x.Id == id);
        _unitOfWork.Commit();

        return RedirectToAction("Index");
    }
    public IActionResult Detail(int? id)
    {
        return View(_unitOfWork.BannerRepo.Get(x => x.Id == id));
    }
    public IActionResult Update(int? id)
    {
        if (id == null) return NotFound();
        var banner = _unitOfWork.BannerRepo.Get(x => x.Id == id);
        if (banner == null) return NotFound();
        BannerVM bannerVM = new BannerVM();
        bannerVM.ImgUrl = banner.ImgUrl;
        bannerVM.Name = banner.Name;
        return View(bannerVM);
    }


    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Update(int? id, BannerVM bannerVM)
    {
        var banner = _unitOfWork.BannerRepo.Get(x => x.Id == id);
        if (bannerVM.Photo != null)
        {
            var exist = _unitOfWork.BannerRepo.Any(c => c.ImgUrl.ToLower() == bannerVM.Photo.FileName.ToLower() && c.Id != id);
            if (!exist)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", banner.ImgUrl);
                DeleteHelper.DeleteFile(path);
                banner.ImgUrl = bannerVM.Photo.FileName;
            }
        }
        banner.Name = bannerVM.Name;
        _unitOfWork.Commit();
        return RedirectToAction("Index");
    }
}