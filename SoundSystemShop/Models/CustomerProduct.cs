namespace SoundSystemShop.Models
{
    public class CustomerProduct: BaseEntity
    {
        public AppUser AppUser { get; set; }
        public double Price { get; set; }
        public string Desc { get; set; }
        public List<CustomerProductImage> ProductImages { get; set; }
        public int RandomNumber { get; set; }
        public string QrCode { get; set; }

        public CustomerProduct()
        {
            ProductImages = new List<CustomerProductImage>();
            IsDeleted = false;
            CreationDate = DateTime.Now;
        }
    }

    public class CustomerProductImage
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
    }
}
