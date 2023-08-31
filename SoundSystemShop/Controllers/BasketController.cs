using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly IProductService _productService;

        public BasketController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View(BasketProducts());
        }
        public IActionResult AddBasket(int id)
        {
            if (id == null) return NotFound();
            var product = _productService.GetProductDetail(id);//_appDbContext.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            List<BasketVM> products;
            if (Request.Cookies["basket"] == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            var existproduct = products.Find(x => x.Id == id);
            if (existproduct == null)
            {
                BasketVM basketVM = new BasketVM()
                {
                    Id = product.Id,
                    Name = product.Name,
                    BasketCount = 1,
                    Price = product.Price,
                    ImgUrl = product.Images.FirstOrDefault().ImgUrl,

                };
                products.Add(basketVM);
            }
            else
            {
                existproduct.BasketCount++;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromMinutes(15) });

            GetBasketCount();
            return NoContent();
        }
        public IActionResult DecreaseBasket(int id)
        {

            if (id == null) return NotFound();
            var product = _productService.GetProductDetail(id);//_appDbContext.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            List<BasketVM> products;
            if (Request.Cookies["basket"] == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            var existproduct = products.Find(x => x.Id == id);
            
                existproduct.BasketCount--;



            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromMinutes(15) });

            GetBasketCount();
            return NoContent();

        }

        public IActionResult RemoveItem(int id)
        {
            if (id == null) return NotFound();

            var products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            var existproduct = products.Find(x => x.Id == id);
            products.Remove(existproduct);
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromMinutes(15) });
            GetBasketCount();
            return NoContent();
        }
        public IActionResult RemoveAllItems()
        {
            var products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            foreach (var item in products)
            {
                RemoveItem(item.Id);
            }
            return NoContent();
        }
        [HttpGet]
        public IActionResult GetBasketCount()
        {
            var basket = Request.Cookies["basket"];
            List<BasketVM> products = string.IsNullOrEmpty(basket)
                ? new List<BasketVM>()
                : JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            int totalCount = products.Sum(item => item.BasketCount);
            return Json(totalCount);
        }

        public IActionResult GetProductCount(int id)
        {
            var basket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket))
            {
                return Json(0);
            }

            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            var existproduct = products.FirstOrDefault(x => x.Id == id);

           
            return Json(existproduct.BasketCount);
        }

        public IActionResult GetTotalPrice()
        {
            var basket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket))
            {
                return Json("0.00");
            }

            var products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            var totalPrice = products.Sum(product => product.Price * product.BasketCount);
            string formattedTotalPrice = totalPrice.ToString("0.00");
            return Json(formattedTotalPrice);
        }

        public IActionResult CheckOut()
        {
            return View(BasketProducts());
        }
        private List<BasketVM> BasketProducts()
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> products;
            if (basket == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
                foreach (var item in products)
                {
                    Product existproduct = _productService.GetProductDetail(item.Id);
                    item.Name = existproduct.Name;
                    item.Price = existproduct.Price;
                    item.ImgUrl = existproduct.Images.FirstOrDefault().ImgUrl;
                }
            }
            return products;
        }
    }
}
