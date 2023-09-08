using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using static QRCoder.PayloadGenerator;
using System.Xml.Linq;

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
            return View(_blogService.GetAllBlogs().Result);
        }
        public IActionResult Detail(int id)
        {
            if (id == null) return RedirectToAction(nameof(Index));
            var blog = _blogService.GetBlogById(id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult CreateBlogComment(int blogId, string comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                if (comment == null)
                {
                    return RedirectToAction(nameof(Detail), new { id = blogId });
                }
                _blogService.CreateBlogComment(blogId, userName, comment);
                return RedirectToAction(nameof(Detail), new { id = blogId });
            }
            else
            {
                string returnUrl = Url.Action("CreateBlogComment", "Blog", new { blogId = blogId, comment = comment });

                return RedirectToAction("Login", "Account", new { ReturnUrl = returnUrl });
            }
        }
        public IActionResult DeleteBlogComment(int blogId, int commentId)
        {
            _blogService.DeleteComment(blogId, commentId);
            return RedirectToAction(nameof(Detail), new {id = blogId});
        }
    }
}
