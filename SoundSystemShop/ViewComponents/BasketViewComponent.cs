using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public BasketViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var product = _appDbContext.Products.Include(p => p.Images).ToList();
            //return View(await Task.FromResult(product));
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


            return View(products);
        }
    }
}
