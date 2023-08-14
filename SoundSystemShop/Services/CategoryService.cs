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
        Task<List<Category>> GetCategories();
        Task AddCategory(CategoryVM categoryVM);
        Task<bool> DeleteCategory(int id);
        Category GetCategoryById(int id);
        CategoryVM MapCategoryVMAndCategory(int id);
        bool UpdateCategory(int id, CategoryVM categoryVM);
        SelectList GetCategorySelectList(int? selectedCategoryId = null);
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

        public SelectList GetCategorySelectList(int? selectedCategoryId = null)
        {
            return new SelectList(_unitOfWork.CategoryRepo.Where(c => c.Id != selectedCategoryId).ToList(), "Id", "Name");
        }

        public Category GetCategoryById(int id)
        {
            return _unitOfWork.CategoryRepo.GetByIdAsync(id).Result;
        }

        public async Task AddCategory(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);

            if (categoryVM.Photo != null)
            {
                category.ImgUrl = categoryVM.Photo.SaveImage(_webHostEnvironment, "assets/img/category");
            }

            await _unitOfWork.CategoryRepo.AddAsync(category);
            _unitOfWork.Commit();
        }

        public bool UpdateCategory(int id, CategoryVM categoryVM)
        {
            var exist = GetCategoryById(id);
            if (exist == null) return false;

            if (exist.ImgUrl != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/category", exist.ImgUrl);
                DeleteHelper.DeleteFile(path);
            }
            if (categoryVM.Photo != null) exist.ImgUrl = categoryVM.Photo.SaveImage(_webHostEnvironment, "assets/img/category");

            exist.Name = categoryVM.Name;
            exist.IsMain = categoryVM.IsMain;
            exist.ParentId = categoryVM.ParentId;
            _unitOfWork.Commit();
            return true;
        }

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

        public CategoryVM MapCategoryVMAndCategory(int id)
        {
            var category = GetCategoryById(id);
            return _mapper.Map<CategoryVM>(category);
        }
    }
}
