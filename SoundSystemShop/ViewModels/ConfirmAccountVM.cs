using System.ComponentModel.DataAnnotations;

namespace SoundSystemShop.ViewModels
{
    public class ConfirmAccountVM
    {
        public string Email { get; set; }
        [Required]
        public string OTP { get; set; }
    }
}
