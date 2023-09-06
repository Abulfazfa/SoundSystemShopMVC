using SoundSystemShop.Models;

namespace SoundSystemShop.ViewModels;

public class CustomerCreateProductVM
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Desc { get; set; }
    public string? Quality { get; set; }
    public List<ProductImage>? Images { get; set; }
    public List<IFormFile>? Photos { get; set; }
}