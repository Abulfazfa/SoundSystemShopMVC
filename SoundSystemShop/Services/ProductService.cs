using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Services
{
    public interface IProductService
    {
        PaginationVM<Product> GetProducts(int page = 1, int take = 15);
        Product GetProductDetail(int id);
        Task<bool> CreateProduct(ProductVM productVM);
        bool UpdateProduct(int id, ProductVM productVM);
        Task<bool> DeleteProduct(int id);
        bool DeleteProductImage(string imgUrl, int id);
        ProductVM MapProductVMAndProduct(int id);
        SelectList GetProductSelectList();
        List<Product> GetAll();
        Product SaleOfDay();
        Sale FinishDateOfSale(string name);
        bool CreateProductComment(int productId, string name, string comment);
        bool DeleteComment(int id, int commentId);

    }
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly PaginationService _paginationService;
        public ProductService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper, PaginationService paginationService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public PaginationVM<Product> GetProducts(int page = 1, int take = 15)
        {
            var query = _unitOfWork.ProductRepo.Queryable();

            query = query.Where(p => p.IsDeleted == false);           

            var products = query
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.Brand != "Customer")
                .Skip(take * (page - 1))
                .Take(take)
                .ToList();

            var productCount = query.Count();
            Discount();

            return new PaginationVM<Product>(products, page, _paginationService.PageCount(productCount, take));
        }

        public List<Product> GetAll()
        {
            Discount();
            return _unitOfWork.ProductRepo.GetProductWithIncludes().Where(p => p.IsDeleted == false && p.Brand != "Customer").OrderByDescending(p => p.CreationDate).ToList();
        }
        public Product GetProductDetail(int id)
        {
            Discount();
            return _unitOfWork.ProductRepo.GetProductWithIncludes().FirstOrDefault(p => p.Id == id);
        }
        public async Task<bool> CreateProduct(ProductVM productVM)
        {
            foreach (var item in productVM.Photos)
            {
                if (!item.CheckFileType())
                {
                    return false;
                }
            }

            Product product = _mapper.Map<Product>(productVM);

            List<ProductImage> images = new();
            foreach (var item in productVM.Photos)
            {
                ProductImage image = new();
                image.ImgUrl = item.SaveImage(_webHostEnvironment, "assets/img/product");
                images.Add(image);
            }
            images.FirstOrDefault().IsMain = true;
            product.Images = images;
            
            List<ProductSpecification> productSpecifications = new List<ProductSpecification>();
            string[] splitArray = productVM.Quality.Split(';');
            foreach (var item in splitArray)
            {
                var productSpecification = new ProductSpecification();
                productSpecification.Name = item.Split('=')[0]; 
                productSpecification.Desc = item.Split('=')[1];
                productSpecifications.Add(productSpecification);
            }
            product.ProductSpecifications = productSpecifications;  
            await _unitOfWork.ProductRepo.AddAsync(product);
            _unitOfWork.Commit();
            return true;
        }
        public bool UpdateProduct(int id, ProductVM productVM)
        {
            var product = _unitOfWork.ProductRepo.GetProductWithIncludes().FirstOrDefault(c => c.Id == id);
            if (product == null)
                return false;

            _mapper.Map<ProductVM, Product>(productVM, product);

            if (productVM.Photos != null)
            {
                var exist =_unitOfWork.ProductRepo
                    .Any(p => productVM.Photos
                    .Any(photo => p.Images.Any(image => image.ImgUrl.ToLower() == photo.FileName.ToLower())) && p.Id != id);

                if (!exist)
                {
                    foreach (var item in product.Images)
                    {
                        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/product", item.ImgUrl);
                        DeleteHelper.DeleteFile(path);
                    }

                    List<ProductImage> images = new();
                    foreach (var item in productVM.Photos)
                    {
                        ProductImage image = new();
                        image.ImgUrl = item.SaveImage(_webHostEnvironment, "assets/img/product");
                        images.Add(image);
                    }
                    images.FirstOrDefault().IsMain = true;
                    product.Images = images;
                }
            }
           

            _unitOfWork.Commit();
            return true;
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var exist = _unitOfWork.ProductRepo.GetProductWithIncludes().FirstOrDefault(p => p.Id == id);
            if (exist == null)
                return false;

            foreach (var item in exist.Images)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/product", item.ImgUrl);
                DeleteHelper.DeleteFile(path);
            }

            await _unitOfWork.ProductRepo.DeleteAsync(exist);
            _unitOfWork.Commit();
            return true;
        }
        public SelectList GetProductSelectList()
        {
            return new SelectList(_unitOfWork.ProductRepo.GetAllAsync().Result, "Id", "Name");
        }
        public bool DeleteProductImage(string imgUrl, int id)
        {
            if (id == null)
                return false;

            var exist = _unitOfWork.ProductRepo.GetProductWithIncludes().FirstOrDefault(p => p.Id == id);
            if (exist == null)
                return false;

            foreach (var item in exist.Images)
            {
                if (item.ImgUrl == imgUrl)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img/product", item.ImgUrl);
                    DeleteHelper.DeleteFile(path);
                }
            }

            var clickedImg = exist.Images.FirstOrDefault(i => i.ImgUrl == imgUrl);
            exist.Images.Remove(clickedImg);

            _unitOfWork.Commit();
            return true;
        }
        public ProductVM MapProductVMAndProduct(int id)
        {
            var product = GetProductDetail(id);
            return _mapper.Map<ProductVM>(product);
        }
        public Product SaleOfDay()
        {
            var sale = _unitOfWork.SaleRepo.GetSaleWithIncludes().FirstOrDefault(s => s.Name == "Daiyly");
            if (sale != null)
            {
                var product = sale.Products.FirstOrDefault();
                return product;
            }
            return null;
        }
        public Sale FinishDateOfSale(string name)
        {
            var sale = _unitOfWork.SaleRepo.GetAllAsync().Result.FirstOrDefault(s => s.Name == name);
            return sale;
        }
        public Sale DateOfSale(string name)
        {
            var sale = _unitOfWork.SaleRepo.GetAllAsync().Result.FirstOrDefault(s => s.Name == name);
            return sale;
        }
        public void Discount(string? saleName = null)
        {
            var sales = _unitOfWork.SaleRepo.GetSaleWithIncludes();

            foreach (var sale in sales)
            {
                if (saleName != null && sale.Name != saleName)
                    continue;

                DateTime startTime = sale.StartDate;
                DateTime endTime = sale.FinishDate;
                bool isInDiscountInterval = IsWithinDiscountInterval(startTime, endTime);
                
                foreach (var product in sale.Products)
                {
                    if (isInDiscountInterval)
                    {
                        product.DiscountPrice = Math.Round(product.Price * ((100 - sale.Percent) / 100));
                        product.InDiscount = true;
                    }
                    else
                    {
                        product.DiscountPrice = product.Price;
                        product.InDiscount = false;
                    }
                }

                _unitOfWork.Commit();
            }
        }

        private bool IsWithinDiscountInterval(DateTime startTime, DateTime endTime)
        {
            DateTime now = DateTime.Now;
            return now >= startTime && now <= endTime;
        }

        public bool CreateProductComment(int productId, string name, string comment)
        {
            var product = GetProductDetail(productId);
            if (product == null) return false;
            ProductComment productComment = new()
            {
                UserName = name,
                UserEmail = "",
                Comment = comment
            };
            product.ProductComments.Add(productComment);
            _unitOfWork.Commit();
            return true;
        }
        public bool DeleteComment(int id, int commentId)
        {
            if (commentId == null) return false;
            var result = GetProductDetail(id);
            if (result == null) return false;
            var clickedComment = result.ProductComments.FirstOrDefault(c => c.Id == commentId);
            result.ProductComments.Remove(clickedComment);
            _unitOfWork.Commit();
            return true;
        }
    }

}
