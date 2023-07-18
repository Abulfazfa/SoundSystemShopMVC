
namespace SoundSystemShop.Models;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }       
    //public DateTime UpdateDate { get; set; }
    public bool IsDeleted { get; set; }
    //public DateTime DeletedDate { get; set; }

    //public Guid DeletedByUserId { get; set; }
    //public Guid UpdateByUserId { get; set; }
    //public Guid CreatedByUserId { get; set; }

    //public virtual AppUser? CreatedByUser { get; set; }
    //public virtual AppUser? DeletedByUser { get; set; }
    //public virtual AppUser? UpdateByUser { get; set; }

    public BaseEntity()
    {
  
        CreationDate = DateTime.Now;            
        //UpdateDate = DateTime.Now;
        //DeletedDate = new DateTime(1, 1, 1);
        IsDeleted = false;

        //DeletedByUserId = Guid.Empty;
        //UpdateByUserId = Guid.Empty;
        //CreatedByUserId = Guid.Empty;

    }
    
}