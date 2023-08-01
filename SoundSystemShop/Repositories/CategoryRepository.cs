using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Repositories.Interfaces;
using SoundSystemShop.Services;

namespace SoundSystemShop.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
