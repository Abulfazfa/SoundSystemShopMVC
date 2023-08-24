namespace SoundSystemShop.Models
{
    public class CustomerProduct: BaseEntity
    {
        public double Price { get; set; }
        public List<ProductImage> Images { get; set; }
        public string Desc { get; set; }
        public int RandomNumber { get; set; }
        public string QrCode { get; set; }

        public CustomerProduct()
        {
            Images = new List<ProductImage>();
            IsDeleted = false;
            CreationDate = DateTime.Now;
        }
    }
}
