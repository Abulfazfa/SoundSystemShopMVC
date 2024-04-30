using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop;
using SoundSystemShop.DAL;
using SoundSystemShop.Hubs;
using SoundSystemShop.Models;
using SoundSystemShop.Profiles;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllersWithViews();
builder.Services.ServiceRegister();
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(option =>
{
    option.AddProfile<MapProfile>();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();


app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name : "areas",
    pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chat");
app.Run();