using Microsoft.AspNetCore.Identity;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.Services;
using SoundSystemShop.Repositories.Interfaces;
using SoundSystemShop.Repositories;

namespace SoundSystemShop
{
    public static class ServiceRegistration
    {
        public static void ServiceRegister(this IServiceCollection services)
        {
            services.AddControllersWithViews(option =>
            {
                option.Filters.Add(typeof(UserActivityFilter));
            });

            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(10);
            });
            services.AddHttpContextAccessor();
            services.AddIdentity<AppUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireDigit = true;

                option.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerProductRepository, CustomerRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<UserActivityFilter>();
            services.AddScoped<PaginationService>();
            services.AddScoped<PromoService>();
            services.AddScoped<GenerateQRCode>();
            services.AddHostedService<PromoEmailSenderHostedService>();
            services.AddSignalR();
        }
    }
}
