namespace SoundSystemShop.Models
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FinishDate { get; set; }
        public PromoCode()
        {
            CreateDate = DateTime.Now;
        }
    }
}
