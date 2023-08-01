using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;

namespace SoundSystemShop.Services
{
    public class UserActivityFilter : IActionFilter
    {
        private readonly AppDbContext _appDbContext;

        public UserActivityFilter(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
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
}
