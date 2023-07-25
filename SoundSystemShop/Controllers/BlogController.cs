using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public IActionResult Index()
        {
            return View(_blogService.GetAllBlogs());
        }
        public IActionResult Detail(int id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var blog = _blogService.GetBlogById(id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult CreateBlogComment(int blogId, string name, string email, string comment)
        {
            _blogService.CreateBlogComment(blogId, name, email, comment);
            return RedirectToAction(nameof(Detail), new {id = blogId});
        }
    }
}
