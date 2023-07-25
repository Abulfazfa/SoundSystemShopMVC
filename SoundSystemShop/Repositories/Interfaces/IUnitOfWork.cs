using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        IProductRepository ProductRepo { get; set; }
        ISliderRepository SliderRepo { get; set; }
        IBannerRepository BannerRepo { get; set; }
        ISocialMediaRepository SocialMediaRepo { get; set; }
        IBlogRepository BlogRepo { get; set; }
        IGenericRepository<AppUser> AppUserRepo { get; }

    }
}
