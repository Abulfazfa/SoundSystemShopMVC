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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserActivityFilter(AppDbContext appDbContext, IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
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
            string pattern = @"Product/\d+";
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

                // Retrieve existing user activities from cookies
                var userActivities = GetUserActivitiesFromCookies(httpContext);

                // Check if an activity with the same URL already exists
                var existingActivity = userActivities.FirstOrDefault(ua => ua.Url == url);
                if (existingActivity != null)
                {
                    // Remove the existing activity
                    userActivities.Remove(existingActivity);
                }

                // Add the new activity
                userActivities.Add(userActivity);

                // Serialize the updated list back to JSON
                var updatedUserActivitiesJson = JsonConvert.SerializeObject(userActivities);

                // Store the updated JSON in cookies
                httpContext.Response.Cookies.Append("UserActivities", updatedUserActivitiesJson);

                int productId = GetProductIdForUserActivity(userActivity.Url);
                if (productId >= 0)
                {
                    ChangeProductRating(productId);
                }
            }
        }


        public List<UserActivity> GetUserActivitiesFromCookies(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.ContainsKey("UserActivities"))
            {
                var userActivitiesJson = httpContext.Request.Cookies["UserActivities"];
                return JsonConvert.DeserializeObject<List<UserActivity>>(userActivitiesJson) ?? new List<UserActivity>();
            }
            return new List<UserActivity>();
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

        public List<Product> GetRecentlyViewedProducts()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var list = new List<Product>();
            List<UserActivity> userActivities = GetUserActivitiesFromCookies(httpContext);
            foreach (var userActivity in userActivities)
            {
                var productId = GetProductIdForUserActivity(userActivity.Url);
                var product = _productService.GetProductDetail(productId);
                if (product != null)
                {
                    list.Add(product);
                }
            }
            list.Reverse();
            return list;
        }

    }
}
