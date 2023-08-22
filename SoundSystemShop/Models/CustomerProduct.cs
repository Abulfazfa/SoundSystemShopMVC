namespace SoundSystemShop.Models
{
    public class CustomerProduct
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public List<ProductImage> Images { get; set; }
        public string Desc { get; set; }
        public string? QrCode { get; set; }

        public CustomerProduct()
        {
            Images = new List<ProductImage>();
        }
    }
}
