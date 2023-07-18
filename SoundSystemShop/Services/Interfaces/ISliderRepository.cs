using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface ISliderRepository : IGenericRepository<Slider>
    {
        bool Any(Func<Slider, bool> func);
    }
}
