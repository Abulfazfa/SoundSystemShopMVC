using Microsoft.AspNetCore.Identity;
using SoundSystemShop.Models;

namespace SoundSystemShop.ViewModels
{
    public class RoleChangeVM
    {
        public AppUser User { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
