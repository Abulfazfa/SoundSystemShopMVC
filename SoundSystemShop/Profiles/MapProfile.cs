using AutoMapper;
using SoundSystemShop.Models;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Profiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Blog, BlogVM>();
            CreateMap<BlogVM, Blog>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.Photo.FileName));
        }
    }

}
