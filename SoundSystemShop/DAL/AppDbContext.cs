using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoundSystemShop.Models;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace SoundSystemShop.DAL;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get;  set; }
    public DbSet<Product> Products { get;  set; }
    public DbSet<ProductImage> ProductImages { get;  set; }
    public DbSet<ProductSpecification> ProductSpecifications { get;  set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<SocialMedia> SocialMedias { get; set; }
    public DbSet<SocialMediaImg> SocialMediaImgs { get; set; }
    public DbSet<BlogComment> BlogComments { get; set; }
    public DbSet<ProductComment> ProductComments { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
    public DbSet<UserMessage> UserMessages { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<AdminPromo> AdminPromos { get; set; }
    public DbSet<CustomerProduct> CustomerProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        this.SeedUsers(builder);
        this.SeedRoles(builder);
        this.SeedUserRoles(builder);
    }

    private void SeedUsers(ModelBuilder builder)
    {
        AppUser user = new AppUser()
        {
            Id = "b74ddd14-6340-4840-95c2-db12554843e5",
            UserName = "Admin",
            Fullname = "Admin",
            OTP = "0000",
            Location = "Baku",
            Email = "admin@gmail.com",
            LockoutEnabled = true
        };

        PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
        passwordHasher.HashPassword(user, "Admin@123");

        builder.Entity<AppUser>().HasData(user);
    }

    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            //new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },  
            new IdentityRole() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" },
            new IdentityRole() { Id = "d7b014f0-5201-4137-abd8-c211t91b7370", Name = "Expert", ConcurrencyStamp = "3", NormalizedName = "Expert" }
        );
    }

    private void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
        );
    }
    

}
