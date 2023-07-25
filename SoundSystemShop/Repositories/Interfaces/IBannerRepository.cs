using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IBannerRepository : IGenericRepository<Banner>
    {
        bool ExistsWithImgUrl(string imgUrl, int idToExclude);
    }
}
