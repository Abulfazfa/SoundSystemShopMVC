using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SoundSystemShop.Models
{
    public class Order
    {

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
        public int Id { get; set; }
        [Display(Name = "Order No")]
        public string OrderNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
