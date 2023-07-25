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
    private readonly ISliderService _sliderService;
    public SliderController(ISliderService sliderService)
    {
        _sliderService = sliderService;
    }



    public IActionResult Index()
    {
        var sliders = _sliderService.GetAllSliders().Result.ToList();
        return View(sliders);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(SliderVM sliderVM)
    {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }
        _sliderService.CreateSlider(sliderVM);
        return RedirectToAction("Index");
    }
    public IActionResult Delete(int id)
    {
        _sliderService.DeleteSlider(id);
        return RedirectToAction("Index");
    }
    public IActionResult Detail(int id)
    {
        return View(_sliderService.GetSliderById(id));
    }
    public IActionResult Update(int id)
    {
        var sliderVM = _sliderService.MapSliderVMAndSlider(id);
        return View(sliderVM);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Update(int id, SliderVM sliderVM)
    {
        if(_sliderService.UpdateSlider(id, sliderVM).Result) return RedirectToAction("Index");
        return View(sliderVM);

    }
}