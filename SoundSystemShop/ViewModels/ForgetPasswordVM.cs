using System.ComponentModel.DataAnnotations;

namespace SoundSystemShop.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
