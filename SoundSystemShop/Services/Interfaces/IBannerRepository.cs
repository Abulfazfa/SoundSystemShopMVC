using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IBannerRepository : IGenericRepository<Banner>
    {
        public bool Any(Func<Banner, bool> func);
    }
}
