using SoundSystemShop.Models;
using System.Reflection.Metadata;
using System.Reflection;

namespace SoundSystemShop.ViewModels
{
    public class HomeVW
    {
        public List<Slider> Sliders { get; set; }
        public List<Banner> Banners { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<SocialMedia> SocialMedias { get; set; }
        public List<Product> UserActivities { get; set; }
        public Product DayOfDiscount { get; set; }
    }
}
