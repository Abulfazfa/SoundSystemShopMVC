namespace SoundSystemShop.ViewModels
{
    public class CategoryVM
    {
        public IFormFile? Photo { get; set; }
        public string? ImgUrl { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public Nullable<int> ParentId { get; set; }
    }
}
