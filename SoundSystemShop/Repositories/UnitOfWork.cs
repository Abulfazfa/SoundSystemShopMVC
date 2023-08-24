using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Repositories.Interfaces;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IProductRepository ProductRepo { get ; set ; }
        public ICategoryRepository CategoryRepo { get ; set ; }
        public ISliderRepository SliderRepo { get; set; }
        public IBannerRepository BannerRepo { get; set; }
        public ISocialMediaRepository SocialMediaRepo { get ; set ; }
        public IBlogRepository BlogRepo { get ; set ; }
        public IGenericRepository<AppUser> AppUserRepo { get; private set; }
        public ISaleRepository SaleRepo { get ; set ; }
        public ICustomerProductRepository CustomerProductRepo { get ; set ; }

        public UnitOfWork(AppDbContext appDbContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, ICategoryRepository categoryRepo, ISaleRepository saleRepo, ICustomerProductRepository customerProductRepo)
        {
            _appDbContext = appDbContext;
            SliderRepo = new SliderRepository(_appDbContext);
            ProductRepo = new ProductRepository(_appDbContext);
            BannerRepo = new BannerRepository(_appDbContext);
            SocialMediaRepo = new SocialMediaRepository(_appDbContext);
            BlogRepo = new BlogRepository(_appDbContext);
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            AppUserRepo = new GenericRepository<AppUser>(_appDbContext);
            CategoryRepo = categoryRepo;
            SaleRepo = saleRepo;
            CustomerProductRepo = customerProductRepo;
        }

        public void Commit()
        {
            _appDbContext.SaveChanges();
        }
    }
}
