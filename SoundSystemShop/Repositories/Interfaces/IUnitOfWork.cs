namespace SoundSystemShop.Services.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        public IProductRepository ProductRepo { get; set; }
        public ISliderRepository SliderRepo { get; set; }
        public IBannerRepository BannerRepo { get; set; }
        public ISocialMediaRepository SocialMediaRepo { get; set; }
        public IBlogRepository BlogRepo { get; set; }
    }
}
