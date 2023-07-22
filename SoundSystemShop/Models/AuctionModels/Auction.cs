using System.ComponentModel.DataAnnotations;
using System;

namespace SoundSystemShop.Models.AuctionModels
{
    public class Auction
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        [Range(0, Double.MaxValue)]
        public double StartingPrice { get; set; }
        [Required]
        //public ICollection<Offer> Offers { get; set; }
        //public ArtWork ArtWork { get; set; }
        //[Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
