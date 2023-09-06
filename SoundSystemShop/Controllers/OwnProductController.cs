using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;
using System.Data;
using System.Linq;

namespace SoundSystemShop.Controllers
{
    [Authorize]
    public class OwnProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenerateQRCode _generateQRCode;
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        public OwnProductController(IProductService productService, IUnitOfWork unitOfWork, GenerateQRCode generateQRCode, AppDbContext appDbContext, IFileService fileService, IEmailService emailService)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
            _generateQRCode = generateQRCode;
            _appDbContext = appDbContext;
            _fileService = fileService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var cproducts = _appDbContext.Products.Include(cp => cp.Images).ToList();
            var userProducts = new List<Product>();
            foreach (var item in cproducts)
            {
                if (item.Name.Split(" -")[0] == User.Identity.Name)
                {
                    userProducts.Add(item);
                }
            }
            return View(userProducts);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProduct)
        {
            
            if (createProduct == null) return Json(null);
            Product product = new Product();
            List<Product> products = new();
            var frame = _productService.GetAll().FirstOrDefault(p => p.Name == createProduct.Frame.Split(" -")[0]);
            var subwoofer = _productService.GetAll().FirstOrDefault(p => p.Name == createProduct.Subwoofer.Split(" -")[0]);
            var horn = _productService.GetAll().FirstOrDefault(p => p.Name == createProduct.Horn.Split(" -")[0]);
            var more = _productService.GetAll().FirstOrDefault(p => p.Name == createProduct.More.Split(" -")[0]);
            products.Add(more);
            products.Add(frame);
            products.Add(subwoofer);
            products.Add(horn);
            product.Price = products.Sum(p => p.Price);
            product.Desc = string.Join("; ", products.ConvertAll(p => p.Name));
            List<ProductImage> productImages = new List<ProductImage>();
            foreach (var item in products)
            {
                ProductImage image = new ProductImage();
                image.ImgUrl = item.Images.FirstOrDefault().ImgUrl;
                productImages.Add(image);
            }
            
            product.Images = productImages;
             Random random = new Random();
             int randomNumber = random.Next(100000, 1000000);
            product.Name = User.Identity.Name + " - " + randomNumber;
            product.ProductCount = 1;
            product.CategoryId = 3;
            product.InDiscount = false;
            product.IsDeleted = true;
            await _unitOfWork.ProductRepo.AddAsync(product);
            
            // customerProduct.RandomNumber = randomNumber;
            // customerProduct.QrCode = _generateQRCode.GenerateQR("https://localhost:44392/OwnProduct/SpecialProducts?randomNumber=" + randomNumber);
            // customerProduct.AppUser = _unitOfWork.AppUserRepo.GetAllAsync().Result.FirstOrDefault(u => u.UserName == createProduct.UserName);
            // await _unitOfWork.CustomerProductRepo.AddAsync(customerProduct);
            _unitOfWork.Commit();
            return Json(product);
        }
        public IActionResult Detail(int id)
        {
            var cproduct = _appDbContext.Products.Include(cp => cp.Images).FirstOrDefault(cp => cp.Id == id);
            if (cproduct == null) return NotFound();
            return View(cproduct);
        }
        public IActionResult Delete(int id)
        {
            var cproduct = _appDbContext.Products.Include(cp => cp.Images).FirstOrDefault(cp => cp.Id == id);
            if (cproduct == null) return NotFound();
            _appDbContext.Products.Remove(cproduct);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SpecialProducts(string name)
        {
            if (string.IsNullOrEmpty(name)) return NotFound();
            var cproduct = _appDbContext.Products.Include(cp => cp.Images).FirstOrDefault(cp => cp.Name == name);
            if(cproduct == null) return NotFound();
            return View(cproduct);
        }
        public IActionResult ShareProduct(string email)
        {
            
            string body = string.Empty;
            string path = "wwwroot/template/verify.html";
            string subject = "Get Promo code";
            body = _fileService.ReadFile(path, body);
            body = body.Replace("{{Confirm Account}}", "Go to");
            body = body.Replace("{{Welcome!}}", User.Identity.Name);
            body = body.Replace("{{link!}}", User.Identity.Name);
            _emailService.Send(email, subject, body);
            return NoContent();
        }
        public IActionResult Order(int id)
        {
            if (id <= 0) return NotFound();
            UserMessage userMessage = new()
            {
                UserName = User.Identity.Name,
                Subject = "Create New Product",
                Message = $"{id}",
            };
            _appDbContext.UserMessages.Add(userMessage);
            _appDbContext.SaveChanges();
            return Content("Send successfully");
        }
        public IActionResult ConfirmedProduct()
        {
            var cproducts = _appDbContext.Products.Include(cp => cp.Images).ToList();
            var userProducts = new List<Product>();
            foreach (var item in cproducts)
            {
                if (item.Name.Split(" -")[0] == User.Identity.Name && item.Brand == "Customer")
                {
                    userProducts.Add(item);
                }
            }
            return View(userProducts);
        }
    }
}
