using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoundSystemShop.Areas.AdminArea.Controllers;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.ViewModels;
using System;
using System.Collections.Generic;

namespace SoundSystemShop.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IProductService _productService;

        public WishlistController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var wishlistItems = HttpContext.Request.Cookies["Wishlist"];
            var wishlist = wishlistItems != null ? JsonConvert.DeserializeObject<List<WishlistVM>>(wishlistItems) : new List<WishlistVM>();

            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                return View(WishlistVMToProduct(wishlist));
            }
            else
            {
                // Handle the case where the user is not authenticated (e.g., display a message)
                return RedirectToAction("Login", "Account"); // Replace with an appropriate view
            }
        }

        public IActionResult AddToWishlist(int productId)
        {
            var wishlistItems = HttpContext.Request.Cookies["Wishlist"];
            var wishlist = wishlistItems != null ? JsonConvert.DeserializeObject<List<WishlistVM>>(wishlistItems) : new List<WishlistVM>();
            var userId = User.Identity.Name;

            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                wishlist.Add(new WishlistVM { ProductId = productId, UserId = userId });

                HttpContext.Response.Cookies.Append("Wishlist", JsonConvert.SerializeObject(wishlist));
                return NoContent();
            }
            else
            {
                // Handle the case where the user is not authenticated (e.g., return an error)
                return BadRequest("User is not authenticated.");
            }
        }

        public IActionResult RemoveFromWishlist(int productId)
        {
            var wishlistItems = HttpContext.Request.Cookies["Wishlist"];
            var wishlist = wishlistItems != null ? JsonConvert.DeserializeObject<List<WishlistVM>>(wishlistItems) : new List<WishlistVM>();

            wishlist.RemoveAll(item => item.ProductId == productId);

            HttpContext.Response.Cookies.Append("Wishlist", JsonConvert.SerializeObject(wishlist));

            return NoContent();
        }
        public IActionResult GetWishlistCount()
        {
            var wishlist = Request.Cookies["Wishlist"];
            List<WishlistVM> products = string.IsNullOrEmpty(wishlist)
                ? new List<WishlistVM>()
                : JsonConvert.DeserializeObject<List<WishlistVM>>(wishlist);

            int totalCount = products.Count;
            return Json(totalCount);
        }

        private List<Product> WishlistVMToProduct(List<WishlistVM> wishlistVMs)
        {
            List<Product> products = new List<Product>();
            var userId = User.Identity.Name;

            foreach (var item in wishlistVMs)
            {
                if (item.UserId == userId)
                {
                    var existProduct = _productService.GetProductDetail(item.ProductId);
                    products.Add(existProduct);
                }
            }

            return products;
        }
        // WishlistController.cs

        [HttpPost] // Use POST method for toggling
        public IActionResult ToggleWishlist(int productId)
        {
            var wishlistItems = HttpContext.Request.Cookies["Wishlist"];
            var wishlist = wishlistItems != null ? JsonConvert.DeserializeObject<List<WishlistVM>>(wishlistItems) : new List<WishlistVM>();
            var userId = User.Identity.Name;

            var existingItem = wishlist.FirstOrDefault(item => item.ProductId == productId && item.UserId == userId);

            if (existingItem != null)
            {
                // Product is in the wishlist, remove it
                wishlist.Remove(existingItem);
                HttpContext.Response.Cookies.Append("Wishlist", JsonConvert.SerializeObject(wishlist));
                return Json(new { isInWishlist = false });
            }
            else
            {
                // Product is not in the wishlist, add it
                wishlist.Add(new WishlistVM { ProductId = productId, UserId = userId });
                HttpContext.Response.Cookies.Append("Wishlist", JsonConvert.SerializeObject(wishlist));
                return Json(new { isInWishlist = true });
            }
        }

    }
}
