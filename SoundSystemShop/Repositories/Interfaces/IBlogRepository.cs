using SoundSystemShop.Models;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
         Task<Blog> GetBlogWithComments(int blogId);
    }
}
