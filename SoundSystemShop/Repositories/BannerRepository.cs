using Microsoft.EntityFrameworkCore;
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
        public bool ExistsWithImgUrl(string imgUrl, int idToExclude)
        {
            return Any(b => b.ImgUrl.ToLower() == imgUrl.ToLower() && b.Id != idToExclude);
        }
    }
}
