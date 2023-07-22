namespace SoundSystemShop.Models
{
    public class SocialMedia : BaseEntity
    {
        public string Name { get; set; }
        public List<SocialMediaImg> ImgUrls { get; set; }
        public string Desciption { get; set; }
        public string Tag { get; set; }

        public SocialMedia()
        {
            ImgUrls = new List<SocialMediaImg>();
        }
    }
    public class SocialMediaImg
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}
