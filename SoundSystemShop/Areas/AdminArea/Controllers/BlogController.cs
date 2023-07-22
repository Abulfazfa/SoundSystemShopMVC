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
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BlogController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _webHostEnvironment = webHostEnvironment;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        return View(_unitOfWork.BlogRepo.GetAll());
    }
    public IActionResult Detail(int? id)
    {
        return View(_unitOfWork.BlogRepo.Get(b => b.Id == id));
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Create(BlogVM blogVM)
    {

        if (!blogVM.Photo.CheckFileType())
        {
            ModelState.AddModelError("Photo", "Sellect a image");
            return View();
        }

        Blog blog = _mapper.Map<Blog>(blogVM);
        _unitOfWork.BlogRepo.Add(blog);
        _unitOfWork.Commit();

        return RedirectToAction("Index");
    }
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();
        var blog = _unitOfWork.BlogRepo.Get(x => x.Id == id);
        if (blog == null) return NotFound();

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/blog", blog.ImgUrl);
        DeleteHelper.DeleteFile(path);
        _unitOfWork.BlogRepo.Delete(b => b.Id == blog.Id);
        _unitOfWork.Commit();

        return RedirectToAction("Index");
    }

    public IActionResult Update(int? id)
    {
        ViewBag.Id = id;
        if (id == null) return NotFound();
        var blog = _unitOfWork.BlogRepo.Get(x => x.Id == id);
        if (blog == null) return NotFound();
        BlogVM blogVM = _mapper.Map<BlogVM>(blog);
        return View(blogVM);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult Update(int? id, BlogVM blogVM)
    {
        var blog = _unitOfWork.BlogRepo.Get(x => x.Id == id);
        DateTime existingCreationDate = blog.CreationDate;
        if (blogVM.Photo != null)
        {
            var exist = _unitOfWork.BlogRepo.Any(c => c.ImgUrl.ToLower() == blogVM.Photo.FileName.ToLower() && c.Id != id);
            if (!exist)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/blog", blog.ImgUrl);
                DeleteHelper.DeleteFile(path);
                blog.ImgUrl = blogVM.Photo.FileName;
            }
        }
        _mapper.Map<Blog>(blogVM);
        blog.CreationDate = existingCreationDate;
        _unitOfWork.Commit();
        return RedirectToAction("Index");
    }

    public IActionResult DeleteComment(int? id, int commentId)
    {
        if (id == null) return View();
        var exist = _unitOfWork.BlogRepo.Get(b => b.Id == id);
        if (exist == null) return View();
        var clickedComment = exist.Comments.FirstOrDefault(c => c.Id == commentId);
        exist.Comments.Remove(clickedComment);

        _unitOfWork.Commit();
        return RedirectToAction("Update", new { Id = id });
    }
}