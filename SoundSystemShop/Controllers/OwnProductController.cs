using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;
using System.Linq;

namespace SoundSystemShop.Controllers
{
    public class OwnProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenerateQRCode _generateQRCode;
        private readonly AppDbContext _appDbContext;
        private readonly EmailService _emailService;
        private readonly FileService _fileService;
        public OwnProductController(IProductService productService, IUnitOfWork unitOfWork, GenerateQRCode generateQRCode, AppDbContext appDbContext, EmailService emailService, FileService fileService)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
            _generateQRCode = generateQRCode;
            _appDbContext = appDbContext;
            _emailService = emailService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var cproducts = _appDbContext.CustomerProducts.Include(cp => cp.Images).ToList();
            return View(cproducts);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProduct)
        {
            if (createProduct == null) return Json(null);
            CustomerProduct customerProduct = new CustomerProduct();
            List<Product> products = new();
            var frame = _productService.GetAll().FirstOrDefault(p => p.Name == createProduct.Frame.Split(" -")[0]);
            var subwoofer = _productService.GetAll().FirstOrDefault(p => p.Name == createProduct.Subwoofer.Split(" -")[0]);
            var horn = _productService.GetAll().FirstOrDefault(p => p.Name == createProduct.Horn.Split(" -")[0]);
            var more = _productService.GetAll().FirstOrDefault(p => p.Name == createProduct.More.Split(" -")[0]);
            products.Add(more);
            products.Add(frame);
            products.Add(subwoofer);
            products.Add(horn);
            customerProduct.Price = products.Sum(p => p.Price);
            customerProduct.Desc = string.Join("; ", products.ConvertAll(p => p.Name));
            foreach (var item in products)
            {
                customerProduct.Images.Add(item.Images.FirstOrDefault());
            }
            Random random = new Random();
            int randomNumber = random.Next(100000, 1000000);
            customerProduct.RandomNumber = randomNumber;
            customerProduct.QrCode = _generateQRCode.GenerateQR("https://localhost:44392/SpecialProducts?randomNumber=" + randomNumber);
            await _unitOfWork.CustomerProductRepo.AddAsync(customerProduct);
            _unitOfWork.Commit();
            return Json(customerProduct);
        }

        public IActionResult Detail(int id)
        {
            var cproduct = _appDbContext.CustomerProducts.Include(cp => cp.Images).FirstOrDefault(cp => cp.Id == id);
            if (cproduct == null) return NotFound();
            return View(cproduct);
        }
        public IActionResult Delete(int id)
        {
            var cproduct = _appDbContext.CustomerProducts.FirstOrDefault(cp => cp.Id == id);
            if (cproduct == null) return NotFound();
            _appDbContext.CustomerProducts.Remove(cproduct);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SpecialProducts(int randomNumber)
        {
            if (randomNumber <= 0) return NotFound();
            var cproduct = _appDbContext.CustomerProducts.Include(cp => cp.Images).FirstOrDefault(cp => cp.Id == randomNumber);
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
    }
}
