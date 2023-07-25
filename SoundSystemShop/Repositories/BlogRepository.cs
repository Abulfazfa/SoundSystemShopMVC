using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using System.Linq;

namespace SoundSystemShop.Services
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
        public bool ExistsWithImgUrl(string imgUrl, int idToExclude)
        {
            return true;
        }
    }
}
