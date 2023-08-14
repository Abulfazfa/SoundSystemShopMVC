using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByPredicateAsync(Predicate<T> func);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        bool Any(Func<T, bool> func);
        List<T> Where(Func<T, bool> func);
        IQueryable Queryable();
    }
}
