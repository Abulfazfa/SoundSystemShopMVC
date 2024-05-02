using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;
using Stripe.Checkout;

namespace SoundSystemShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly IProductService _productService;
        private readonly PromoService _promoService;

        public BasketController(IProductService productService, PromoService promoService)
        {
            _productService = productService;
            _promoService = promoService;
        }

        public IActionResult Index()
        {
            return View(BasketProducts());
        }
        public IActionResult AddBasket(int id)
        {
            if (id == null) return NotFound();
            var product = _productService.GetProductDetail(id);
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
                    Price = product.DiscountPrice,
                    ImgUrl = product.Images.FirstOrDefault().ImgUrl,

                };
                products.Add(basketVM);
            }
            else
            {
                existproduct.BasketCount++;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) });

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
        public IActionResult FirstCheckOut()
        {
            return View(BasketProducts());
        }
        public IActionResult CheckOut(int price)
        {
            var domain = "https://localhost:44392/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Basket/Success",
                CancelUrl = domain + "Basket/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                Locale = "en"
            };

            
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Pay"
                        }
                    },
                    Quantity = 1,
                };

                options.LineItems.Add(sessionListItem);
            

            var service = new SessionService();
            try
            {
                Session session = service.Create(options);
                TempData["Session"] = session.Id;
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }
        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "Paid")
            {
                return View("Success");
            }
            return View("Fail");
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Fail()
        {
            return View();
        }
        public IActionResult GetPromo(string promo)
        {
            return Json(_promoService.GetPromo(promo));

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
                    item.Price = existproduct.DiscountPrice;
                    item.ImgUrl = existproduct.Images.FirstOrDefault().ImgUrl;
                }
            }
            return products;
        }
    }
}
