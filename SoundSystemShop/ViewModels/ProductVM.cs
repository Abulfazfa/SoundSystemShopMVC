using SoundSystemShop.Models;

namespace SoundSystemShop.ViewModels
{
    public class ProductVM
    {
        public string Name { get; set; }
        public double? DiscountPrice { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
        public string? Brand { get; set; }
        public string? Quality { get; set; }
        public int ProductCount { get; set; }
        public int CategoryId { get; set; }
        public List<ProductImage>? Images { get; set; }
        public List<IFormFile>? Photos { get; set; }
    }
}
