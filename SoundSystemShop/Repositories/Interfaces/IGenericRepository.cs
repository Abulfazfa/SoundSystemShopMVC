using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByPredicateAsync(Func<T,bool> func);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        bool Any(Func<T, bool> func);
    }
}
