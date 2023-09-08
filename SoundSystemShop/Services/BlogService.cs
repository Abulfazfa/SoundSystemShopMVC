using AutoMapper;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;
using System.ComponentModel.Design;
using static QRCoder.PayloadGenerator;
using System.Xml.Linq;

namespace SoundSystemShop.Services
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAllBlogs();
        Task CreateBlog(BlogVM blogVM);
        Task<bool> DeletBlog(int id);
        Blog GetBlogById(int id);
        BlogVM MapBlogVMAndBlog(int id);
        Task<bool> UpdateBlog(int id, BlogVM blogVM);
        bool CreateBlogComment(int blogId, string name, string comment);
        bool DeleteComment(int id, int commentId);
    }
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public BlogService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        public async Task CreateBlog(BlogVM blogVM)
        {
            if (!blogVM.Photo.CheckFileType())
            {
                throw new ArgumentException("Select an image.");
            }

            Blog blog = _mapper.Map<Blog>(blogVM);
            blog.ImgUrl = blogVM.Photo.SaveImage(_webHostEnvironment, "assets/img/blog");
            await _unitOfWork.BlogRepo.AddAsync(blog);
            _unitOfWork.Commit();
        }

        public async Task<bool> DeletBlog(int id)
        {
            var blog = _unitOfWork.BlogRepo.GetByIdAsync(id).Result;

            if (blog != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/blog", blog.ImgUrl);
                DeleteHelper.DeleteFile(path);
                await _unitOfWork.BlogRepo.DeleteAsync(blog);
                _unitOfWork.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            return await _unitOfWork.BlogRepo.GetAllAsync();
        }

        public Blog GetBlogById(int id)
        {
            return _unitOfWork.BlogRepo.GetBlogWithComments(id).Result;
        }

        public BlogVM MapBlogVMAndBlog(int id)
        {
            var blog = GetBlogById(id);
            if (blog == null) return null;
            BlogVM blogVM = _mapper.Map<BlogVM>(blog);
            blogVM.ImgUrl = blog.ImgUrl;
            return blogVM;
        }

        public async Task<bool> UpdateBlog(int id, BlogVM blogVM)
        {
            var blog = await _unitOfWork.BlogRepo.GetByIdAsync(id);
            blog.Author = blogVM.Author;
            blog.Description = blogVM.Description;
            blog.Name = blogVM.Name;
            DateTime existingCreationDate = blog.CreationDate;
            if (blogVM.Photo != null)
            {
                bool exist = _unitOfWork.BlogRepo.Any(b => b.ImgUrl.ToLower() == blogVM.Photo.FileName.ToLower() && b.Id != id);
                if (true)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/blog", blog.ImgUrl);
                    DeleteHelper.DeleteFile(path);
                    blog.ImgUrl = blogVM.Photo.SaveImage(_webHostEnvironment, "assets/img/blog");
                }
            }
            _unitOfWork.Commit();
            return true;
        }
        public bool DeleteComment(int id, int commentId)
        {
            if (commentId == null) return false;
            var result = GetBlogById(id);
            if (result == null) return false;
            var clickedComment = result.Comments.FirstOrDefault(c => c.Id == commentId);
            result.Comments.Remove(clickedComment);
            _unitOfWork.Commit();
            return true;
        }
        public bool CreateBlogComment(int blogId, string name, string comment)
        {
            var blog = GetBlogById(blogId);
            if (blog == null) return false;
            BlogComment blogComment = new()
            {
                UserName = name,
                UserEmail = "",
                Comment = comment
            };
            blog.Comments.Add(blogComment);
            _unitOfWork.Commit();
            return true;
        }
        
    }
}
