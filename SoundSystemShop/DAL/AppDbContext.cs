using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;

namespace SoundSystemShop.DAL;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get;  set; }
    public DbSet<Product> Products { get;  set; }
    public DbSet<ProductImage> ProductImages { get;  set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<SocialMedia> SocialMedias { get; set; }
    public DbSet<SocialMediaImg> SocialMediaImgs { get; set; }
    public DbSet<BlogComment> BlogComments { get; set; }
}