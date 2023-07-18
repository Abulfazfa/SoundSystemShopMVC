using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Services.Interfaces;

namespace SoundSystemShop.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        
        internal DbSet<T> _dbSet;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<T>();
        }

        public bool Add(T entity)
        {
            _dbSet.Add(entity);
            return true;
        }

        public bool Delete(Func<T, bool> func)
        {
            _dbSet.Remove(_dbSet.FirstOrDefault(func));
            return true;
        }

        public T Get(Func<T, bool> func)
        {
            return _dbSet.FirstOrDefault(func);
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }

    }
}
