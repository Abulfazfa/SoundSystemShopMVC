using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Repositories.Interfaces
{
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        public bool ExistsWithImgUrl(string imgUrl, int idToExclude);
        List<Sale> GetSaleWithIncludes();
    }
}
