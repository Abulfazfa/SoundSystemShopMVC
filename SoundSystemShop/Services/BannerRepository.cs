using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using System.Linq;

namespace SoundSystemShop.Services
{
    public class BannerRepository : GenericRepository<Banner>, IBannerRepository
    {
        public BannerRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
        public bool Any(Func<Banner, bool> func)
        {
            return _dbSet.Any(func);
        }
    }
}
