using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Services
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _dbContext = appDbContext;
        }

        public List<Product> GetProductWithIncludes()
        {
            return _dbContext.Products.Include(p => p.Images).Include(p => p.Category).Include(p => p.ProductComments).Include(p => p.ProductSpecifications).ToList();
        }

        public IQueryable<Product> Queryable()
        {
            return _dbContext.Products;
        }

    }
}
