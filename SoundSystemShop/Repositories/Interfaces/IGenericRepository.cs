using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        T Get(Func<T, bool> func);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(Func<T, bool> func);
        bool Any(Func<T, bool> func);
    }
}
