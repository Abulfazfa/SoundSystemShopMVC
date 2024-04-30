using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SoundSystemShop.DAL;
using SoundSystemShop.Migrations;
using SoundSystemShop.Models;
using SoundSystemShop.ViewModels;
using System.IO;
using System.Text.RegularExpressions;

namespace SoundSystemShop.Services
{
    public class UserActivityFilter : IActionFilter
    {
        private readonly AppDbContext _appDbContext;
        private readonly IProductService _productService;

        public UserActivityFilter(AppDbContext appDbContext, IProductService productService)
        {
            _appDbContext = appDbContext;
            _productService = productService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var controllerName = context.RouteData.Values["controller"];
			var actionName = context.RouteData.Values["action"];
			var id = context.RouteData.Values["id"];
			var url = $"{controllerName}/{actionName}/{id}";

			string data = context.HttpContext.Request.QueryString.HasValue ? context.HttpContext.Request.QueryString.Value : "";

			var userData = context.ActionArguments.FirstOrDefault().Value;
			if (userData != null)
			{
				data = JsonConvert.SerializeObject(userData);
			}

			var userName = context.HttpContext.User.Identity.Name;
			var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
			StoreUserActivity(context.HttpContext, data, url, userName, ipAddress);
		}
		public void StoreUserActivity(HttpContext httpContext, string data, string url, string? userName, string ipAddress)
		{
			string pattern = @".*Product/\d+.*|Product/\d+.*";
			Match match = Regex.Match(url, pattern);
			if (match.Success)
			{
				var userActivity = new UserActivity
				{
					UserName = userName,
					IpAddress = ipAddress,
					Data = data,
					Url = url,
				};

                int productId = GetProductIdForUserActivity(userActivity.Url);
                if (productId < 0) { } 
                var activityJson = JsonConvert.SerializeObject(productId);
				httpContext.Response.Cookies.Append("UserActivity", activityJson, new CookieOptions
				{
					Expires = DateTimeOffset.UtcNow.AddDays(30), // Set expiration as needed
					HttpOnly = true, // Secure flag to prevent JS access (adjust as needed for your security requirements)
				});

                ChangeProductRating(productId);
			}
		}
		
		



		public int GetProductIdForUserActivity(string path)
        {
			int startIndex = path.LastIndexOf('/') + 1;
			int numberPart = int.Parse(path.Substring(startIndex));

			var exist = _productService.GetProductDetail(numberPart);
			if (exist != null)
			{
				return exist.Id;
			}

            return -1;
		}

        public void ChangeProductRating(int productId)
        {
			var exist = _productService.GetProductDetail(productId);
			if (exist != null)
			{
                exist.ProductRating++;
                _appDbContext.Products.Update(exist);
			}
		}

    }
}
