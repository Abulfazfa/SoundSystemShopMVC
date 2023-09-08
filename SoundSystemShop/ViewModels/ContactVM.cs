using System.ComponentModel.DataAnnotations;

namespace SoundSystemShop.ViewModels
{
    public class ContactVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(255)]
        public string Message { get; set; }
    }
}
