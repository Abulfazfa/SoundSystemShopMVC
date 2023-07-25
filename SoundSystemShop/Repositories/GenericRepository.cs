using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using System.Linq;

namespace SoundSystemShop.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        public GenericRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _appDbContext.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _appDbContext.Set<T>().Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity)
        {
            _appDbContext.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<T> GetByPredicateAsync(Func<T, bool> func)
        {
            return await _appDbContext.Set<T>().FindAsync(func);
        }
        public bool Any(Func<T, bool> func)
        {
            return _appDbContext.Set<T>().Any(func);
        }

    }
}
