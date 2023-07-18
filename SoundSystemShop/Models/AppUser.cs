using Microsoft.AspNetCore.Identity;

namespace SoundSystemShop.Models;

public class AppUser : IdentityUser
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Fullname { get; set; }
    public bool IsBlocked { get; set; }
    public string ConnectionId { get; set; }
    public DateTime Birthday { get; set; }
    public int OTP { get; set; }
}