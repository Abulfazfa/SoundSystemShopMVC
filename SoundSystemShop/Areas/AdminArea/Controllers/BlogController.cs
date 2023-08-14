using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.DAL;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;
[Area("AdminArea")]
public class BlogController : Controller
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public IActionResult Index()
    {
        return View(_blogService.GetAllBlogs().Result.ToList());
    }
    public IActionResult Detail(int id)
    {
        return View(_blogService.GetBlogById(id));
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Create(BlogVM blogVM)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }
        try
        {
            _blogService.CreateBlog(blogVM);
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
        var result = _blogService.DeletBlog(id).Result;
        if (!result)
        {
            ModelState.AddModelError("", "Happen some problems");
        }
        return RedirectToAction("Index");
    }

    public IActionResult Update(int id)
    {
        ViewBag.Id = id;
        var blogVM = _blogService.MapBlogVMAndBlog(id);
        if (blogVM == null)
        {
            ModelState.AddModelError("", "Happen problem");
            return View();
        }
        return View(blogVM);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Update(int id, BlogVM blogVM)
    {
        ViewBag.Id = id;
        if (_blogService.UpdateBlog(id, blogVM).Result) return RedirectToAction("Index");
        return View(blogVM);

    }

    public IActionResult DeleteComment(int id, int commentId)
    {
        _blogService.DeleteComment(id, commentId);
        return RedirectToAction("Update", new { Id = id });
    }
}