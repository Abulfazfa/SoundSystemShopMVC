using SoundSystemShop.Models;

namespace SoundSystemShop.ViewModels
{
    public class SaleVM
    {
        public IFormFile? Photo { get; set; }
        public string? ImgUrl { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public List<int>? ProductIds { get; set; }
        public double Percent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
