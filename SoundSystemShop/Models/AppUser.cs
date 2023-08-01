using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SoundSystemShop.Models;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; }
    public bool IsBlocked { get; set; }
    public string? ConnectionId { get; set; }
    public DateTime Birthday { get; set; }
    public string OTP { get; set; }
    public string? Location { get; set; }
    public List<Product> OwnProducts { get; set; }

    public AppUser()
    {
        OwnProducts = new List<Product>();
        IsBlocked = false;
    }
}