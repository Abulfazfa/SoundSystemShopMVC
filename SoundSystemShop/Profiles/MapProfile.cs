using AutoMapper;
using SoundSystemShop.Models;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Profiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Slider, SliderVM>();
            CreateMap<SliderVM, Slider>()
                .ForMember(dest => dest.ImgUrl, opt => opt.Ignore());
            CreateMap<Blog, BlogVM>();
            CreateMap<BlogVM, Blog>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ImgUrl, opt => opt.Ignore());
            CreateMap<Banner, BannerVM>();
            CreateMap<BannerVM, Banner>()
                .ForMember(dest => dest.ImgUrl, opt => opt.Ignore());
            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryVM, Category>()
                .ForMember(dest => dest.ImgUrl, opt => opt.Ignore());
            CreateMap<Product, ProductVM>();
            CreateMap<ProductVM, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.DiscountPrice, opt => opt.Ignore());
            CreateMap<Sale, SaleVM>();
            CreateMap<SaleVM, Sale>()
                .ForMember(dest => dest.ImgUrl, opt => opt.Ignore());
        }
    }

}
 