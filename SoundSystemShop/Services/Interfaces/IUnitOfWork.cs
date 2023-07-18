namespace SoundSystemShop.Services.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        public IProductRepository ProductRepo { get; set; }
        public ISliderRepository SliderRepo { get; set; }
        public IBannerRepository BannerRepo { get; set; }
    }
}
