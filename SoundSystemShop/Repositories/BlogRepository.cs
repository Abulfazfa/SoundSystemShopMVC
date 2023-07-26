using Microsoft.EntityFrameworkCore;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using System;
using System.Linq;

namespace SoundSystemShop.Services
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        private readonly AppDbContext _appDbContext;
        public BlogRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Blog> GetBlogWithComments(int blogId)
        {
            return await _appDbContext.Blogs.Include(b => b.Comments).FirstOrDefaultAsync(b => b.Id == blogId);
        }
    }

}
