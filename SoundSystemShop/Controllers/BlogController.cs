using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Controllers
{
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.BlogRepo.GetAll());
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var blog = _unitOfWork.BlogRepo.Get(b => b.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult CreateBlogComment(int blogId, string name, string email, string comment)
        {
            var blog = _unitOfWork.BlogRepo.Get(b => b.Id == blogId);
            BlogComment blogComment = new()
            {
                UserName = name,
                UserEmail = email,
                Comment = comment
            };
            blog.Comments.Add(blogComment);
            _unitOfWork.Commit();

            return Content("salam");
        }
    }
}
