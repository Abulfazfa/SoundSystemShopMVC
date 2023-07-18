using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;

[Area("AdminArea")]
public class SliderController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUnitOfWork _unitOfWork;

    public SliderController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
    {
        _webHostEnvironment = webHostEnvironment;
        _unitOfWork = unitOfWork;
    }



    public IActionResult Index()
    {
        var sliders = _unitOfWork.SliderRepo.GetAll();
        return View(sliders);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(SliderVM sliderVM)
    {
        if (!sliderVM.Photo.CheckFileType())
        {
            ModelState.AddModelError("Photo", "Sellect a image");
            return View();

        }

        Slider slider = new()
        {
            ImgUrl = sliderVM.Photo.SaveImage(_webHostEnvironment, "assets/img/hero"),
            Title = sliderVM.Title,
            Header = sliderVM.Header,
            Description = sliderVM.Description,
        };
        _unitOfWork.SliderRepo.Add(slider);
        _unitOfWork.Commit();

        return RedirectToAction("Index");
    }
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();
        var slider = _unitOfWork.SliderRepo.Get(s => s.Id == id);
        if (slider == null) return NotFound();

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", slider.ImgUrl);
        DeleteHelper.DeleteFile(path);
        _unitOfWork.SliderRepo.Delete(s => s.Id == id);
        _unitOfWork.Commit();

        return RedirectToAction("Index");
    }
    public IActionResult Detail(int? id)
    {
        return View(_unitOfWork.SliderRepo.Get(s => s.Id == id));
    }
    public IActionResult Update(int? id)
    {
        if (id == null) return NotFound();
        var slider = _unitOfWork.SliderRepo.Get(s => s.Id == id);
        if (slider == null) return NotFound();
        SliderVM sliderVM = new SliderVM()
        {
            ImgUrl = slider.ImgUrl,
            Header = slider.Header,
            Title = slider.Title,
            Description = slider.Description
        };
        ViewBag.SliderImgUrl = slider.ImgUrl;
        return View(sliderVM);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Update(int? id, SliderVM sliderVM)
    {
        var slider = _unitOfWork.SliderRepo.Get(s => s.Id == id);
        if (sliderVM.Photo != null)
        {
            var exist = _unitOfWork.SliderRepo.Any(s => s.ImgUrl.ToLower() == sliderVM.Photo.FileName.ToLower() && s.Id != id);

            if (!exist)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/hero", slider.ImgUrl);
                DeleteHelper.DeleteFile(path);
                slider.ImgUrl = sliderVM.Photo.FileName;
            }
        }

        slider.Header = sliderVM.Header;
        slider.Title = sliderVM.Title;
        slider.Description = sliderVM.Description;

        _unitOfWork.Commit();
        return RedirectToAction("Index");
    }
}