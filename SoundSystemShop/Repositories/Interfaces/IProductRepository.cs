using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable<Product> Queryable();
        List<Product> GetProductWithIncludes();
    }
}
