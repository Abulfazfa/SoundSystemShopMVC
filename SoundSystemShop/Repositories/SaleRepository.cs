using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Repositories.Interfaces;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Repositories
{
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        public SaleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public bool ExistsWithImgUrl(string imgUrl, int idToExclude)
        {
            return Any(b => b.ImgUrl.ToLower() == imgUrl.ToLower() && b.Id != idToExclude);
        }
    }
}
