using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface ISliderRepository : IGenericRepository<Slider>
    {
        bool ExistsWithImgUrl(string imgUrl, int idToExclude);
    }
}
