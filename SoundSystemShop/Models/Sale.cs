namespace SoundSystemShop.Models
{
    public class Sale : BaseEntity
    {
        public string? ImgUrl { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Percent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public List<Product> Products { get; set; }

        public Sale()
        {
            IsDeleted = false;
            CreationDate = DateTime.Now;
            Products = new List<Product>();
        }

    }
}
