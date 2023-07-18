namespace SoundSystemShop.Models
{
    public class Blog : BaseEntity
    {
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public List<BlogComment> Comments { get; set; }

        public Blog()
        {
            Comments = new List<BlogComment>();
        }
    }
}
