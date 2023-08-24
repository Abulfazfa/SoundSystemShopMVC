using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Repositories.Interfaces;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Repositories
{
    public class CustomerRepository : GenericRepository<CustomerProduct>, ICustomerProductRepository
    {
        public CustomerRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
