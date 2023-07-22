namespace SoundSystemShop.Models;

public class BlogComment : BaseEntity
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string Comment { get; set; }
}