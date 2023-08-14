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
        PaginationVM<Product> GetProducts(int page = 1, int take = 2);
        Product GetProductDetail(int id);
        Task<bool> CreateProduct(ProductVM productVM);
        bool UpdateProduct(int id, ProductVM productVM);
        Task<bool> DeleteProduct(int id);
        bool DeleteProductImage(string imgUrl, int id);
        ProductVM MapProductVMAndProduct(int id);
        SelectList GetProductSelectList();
        List<Product> GetAll();
        Product SaleOfDay();
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

        public PaginationVM<Product> GetProducts(int page = 1, int take = 2)
        {
            var query = _unitOfWork.ProductRepo.Queryable();
            var products = query
                .Include(p => p.Images)
                .Include(p => p.Category)
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
            return _unitOfWork.ProductRepo.GetProductWithIncludes().ToList();
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
            //var sale = _unitOfWork.SaleRepo.GetAllAsync().Result.FirstOrDefault(s => s.Name == "SaleOfDay");
            //if (sale != null)
            //{
            //    var percent = sale.Percent;
            //    var product = sale.Products.FirstOrDefault();

            //    if (IsWithinDiscountInterval(sale.StartDate, sale.FinishDate))
            //    {
            //        product.DiscountPrice = product.Price * (percent / 100);
            //        product.InDiscount = true;
            //        string script = $"startCountdown('{sale.FinishDate.ToString("yyyy-MM-ddTHH:mm:ss")}')";
            //    }
            //    else
            //    {
            //        product.DiscountPrice = product.Price;
            //        product.InDiscount = false;
            //    }
            //    _unitOfWork.Commit();
            //    return product;
            //}
            return null;
        }


        public void Discount()
        {
            foreach (var item in _unitOfWork.SaleRepo.GetAllAsync().Result)
            {
                DateTime startTime = item.StartDate;
                DateTime endTime = item.FinishDate;

                if (item.Products.Count != 0)
                {
                    foreach (var product in item.Products)
                    {
                        if (IsWithinDiscountInterval(startTime, endTime))
                        {
                            product.DiscountPrice = product.Price * (item.Percent / 100);
                            product.InDiscount = true;
                        }
                        else
                        {
                            product.DiscountPrice = product.Price;
                            product.InDiscount = false;
                        }
                    }
                }
                else
                {
                    foreach (var product in _unitOfWork.ProductRepo.GetProductWithIncludes().ToList())
                    {
                        if (IsWithinDiscountInterval(startTime, endTime))
                        {
                            product.DiscountPrice = product.Price * (item.Percent / 100);
                            product.InDiscount = true;
                        }
                        else
                        {
                            product.DiscountPrice = product.Price;
                            product.InDiscount = false;
                        }
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
        //public bool CreateProductComment(int productId, string name, string email, string comment)
        //{
        //    var product = GetProductDetail(productId);
        //    if (product == null) return false;
        //    BlogComment blogComment = new()
        //    {
        //        UserName = name,
        //        UserEmail = email,
        //        Comment = comment
        //    };
        //    product.Comments.Add(blogComment);
        //    _unitOfWork.Commit();
        //    return true;
        //}
    }

}
