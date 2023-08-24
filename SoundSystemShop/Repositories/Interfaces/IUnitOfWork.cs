using SoundSystemShop.Models;
using SoundSystemShop.Repositories.Interfaces;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        IProductRepository ProductRepo { get; set; }
        ICategoryRepository CategoryRepo { get; set; }
        ISliderRepository SliderRepo { get; set; }
        IBannerRepository BannerRepo { get; set; }
        ISocialMediaRepository SocialMediaRepo { get; set; }
        ISaleRepository SaleRepo { get; set; }
        IBlogRepository BlogRepo { get; set; }
        ICustomerProductRepository CustomerProductRepo { get; set; }
        IGenericRepository<AppUser> AppUserRepo { get; }

    }
}
