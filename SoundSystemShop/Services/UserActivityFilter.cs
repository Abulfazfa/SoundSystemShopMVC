using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
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
            var data = "";
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            var id = context.RouteData.Values["id"];

            var url = $"{controllerName}/{actionName}/{id}";

            if (string.IsNullOrEmpty(context.HttpContext.Request.QueryString.Value))
            {
                data = context.HttpContext.Request.QueryString.Value;
            }
            else
            {
                var userData = context.ActionArguments.FirstOrDefault();
                var stringUserData = JsonConvert.SerializeObject(userData);
                data = stringUserData;
            }

            var userName = context.HttpContext.User.Identity.Name;
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            StoreUserActivity(data, url, userName, ipAddress);

        }
        public void StoreUserActivity(string data, string url, string? userName, string ipAddress)
        {
            if (url.Contains("Product/"))
            {
                var userActivity = new UserActivity
                {
                    UserName = userName,
                    IpAddress = ipAddress,
                    Data = data,
                    Url = url,
                };
                _appDbContext.UserActivities.Add(userActivity);
                _appDbContext.SaveChanges();
            }
        }
        public List<Product> GetUserActivity(string? username)
        {
            var activities = _appDbContext.UserActivities.ToList(); // USER NAME /////////////////////////////
            List<string> result = new List<string>();
            string pattern = @".*Product/\d+.*|Product/\d+.*";
            foreach (var item in activities)
            {
                Match match = Regex.Match(item.Url, pattern);
                if (match.Success)
                {
                    string matchedPart = match.Value;
                    result.Add(matchedPart); ;
                }
                
            }
            return GetProductsOfUserActivity(result);
        }
        public List<Product> GetProductsOfUserActivity(List<string> url)
        {
            List<Product> result = new List<Product>();
            List<int> processedNumbers = new List<int>();

            foreach (string path in url)
            {
                int startIndex = path.LastIndexOf('/') + 1;
                int numberPart = int.Parse(path.Substring(startIndex));

                // Check if the numberPart has already been processed
                if (!processedNumbers.Contains(numberPart))
                {
                    result.Add(_productService.GetProductDetail(numberPart));
                    processedNumbers.Add(numberPart);
                }
            }

            return result;
        }

    }
}
