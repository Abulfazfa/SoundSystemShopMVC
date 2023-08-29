using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Repositories.Interfaces;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using System;

namespace SoundSystemShop.Repositories
{
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        private readonly AppDbContext _dbContext;
        public SaleRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _dbContext = appDbContext;
        }
        public List<Sale> GetSaleWithIncludes()
        {
            return _dbContext.Sales.Include(s => s.Products).ToList();
        }
        public bool ExistsWithImgUrl(string imgUrl, int idToExclude)
        {
            return Any(b => b.ImgUrl.ToLower() == imgUrl.ToLower() && b.Id != idToExclude);
        }
    }
}
