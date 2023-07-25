using SoundSystemShop.Models;

namespace SoundSystemShop.ViewModels
{
    public class BlogVM
    {
        public IFormFile Photo { get; set; }
        public string? ImgUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public List<BlogComment>? Comments { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
