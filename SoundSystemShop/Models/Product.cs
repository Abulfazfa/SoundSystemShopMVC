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
    public List<ProductSpecification>? ProductSpecifications { get; set; }
    public List<ProductComment>? ProductComments { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
    public bool InDiscount { get; set; }
    public Product()
    {
        Images = new List<ProductImage>();
        ProductComments = new List<ProductComment>();
        ProductSpecifications = new List<ProductSpecification>();
        IsDeleted = false;
        InDiscount = false;
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
public class ProductSpecification : BaseEntity
{
    public string Name { get; set; }    
    public string Desc { get; set; }

    public ProductSpecification()
    {
        IsDeleted = false;
        CreationDate = DateTime.Now;
    }
}
public class ProductComment : BaseEntity
{
    public string UserName { get; set; }
    public string? UserEmail { get; set; }
    public string Comment { get; set; }

    public ProductComment()
    {
        IsDeleted = false;
        CreationDate = DateTime.Now;
    }
}