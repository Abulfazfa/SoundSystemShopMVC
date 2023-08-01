using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SoundSystemShop.DAL;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllBlogs();
        //Task CreateBlog(BlogVM blogVM);
        Task<bool> DeletBlog(int id);
        Category GetBlogById(int id);
        //BlogVM MapBlogVMAndBlog(int id);
        //Task<bool> UpdateBlog(int id, BlogVM blogVM);
        //bool CreateBlogComment(int blogId, string name, string email, string comment);
        //bool DeleteComment(int id, int commentId);
    }
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        public async Task<List<Category>> GetCategories()
        {
            return await _unitOfWork.CategoryRepo.GetAllAsync();
        }

        //public SelectList GetCategorySelectList(int? selectedCategoryId = null)
        //{
        //    return new SelectList(_appDbContext.Categories.Where(c => c.Id != selectedCategoryId).ToList(), "Id", "Name");
        //}

        public Category GetCategoryById(int id)
        {
            return _unitOfWork.CategoryRepo.GetByIdAsync(id).Result;
        }

        //public void AddCategory(CategoryVM categoryVM)
        //{
        //    Category category = new Category
        //    {
        //        Name = categoryVM.Name,
        //        IsMain = categoryVM.IsMain,
        //        ParentId = categoryVM.ParentId,
        //    };

        //    if (categoryVM.Photo != null)
        //    {
        //        category.ImgUrl = categoryVM.Photo.SaveImage(_webHostEnvironment, "images");
        //    }

        //    _appDbContext.Categories.Add(category);
        //    _appDbContext.SaveChanges();
        //}

        //public void UpdateCategory(int? id, CategoryVM categoryVM)
        //{
        //    var exist = _appDbContext.Categories.FirstOrDefault(c => c.Id == id);
        //    if (exist == null) return;

        //    if (categoryVM.Photo != null)
        //    {
        //        string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", exist.ImgUrl);
        //        DeleteHelper.DeleteFile(path);
        //        exist.ImgUrl = categoryVM.Photo.FileName;
        //    }

        //    exist.Name = categoryVM.Name;
        //    exist.IsMain = categoryVM.IsMain;
        //    exist.ParentId = categoryVM.ParentId;
        //    _appDbContext.SaveChanges();
        //}

        public async Task<bool> DeleteCategory(int id)
        {
            var exist = GetCategoryById(id);
            if (exist == null) return false;

            if (exist.ImgUrl != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/category", exist.ImgUrl);
                DeleteHelper.DeleteFile(path);
            }

            await _unitOfWork.CategoryRepo.DeleteAsync(exist);
            _unitOfWork.Commit();
            return true;
        }
    }
}
