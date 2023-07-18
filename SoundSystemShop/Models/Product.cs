namespace SoundSystemShop.Models;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public double Price { get; set; }
    public double DiscountPrice { get; set; }
    public string? Desc { get; set; }
    public string? Brand { get; set; }
    public int ProductCount { get; set; }
    public int? ProductRating { get; set; }
    public List<ProductImage> Images { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
    public Product()
    {
        Images = new List<ProductImage>();
        IsDeleted = false;
    }
}

public class ProductImage : BaseEntity
{
    public string ImgUrl { get; set; }
    public bool IsMain { get; set; }
    public int ProductId { get; set; }
    public ProductImage()
    {
        IsMain = false;
        IsDeleted = false;
    }
}