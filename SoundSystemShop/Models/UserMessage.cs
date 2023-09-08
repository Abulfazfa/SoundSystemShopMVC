namespace SoundSystemShop.Models
{
    public class UserMessage : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool IsSeen { get; set; }
        public UserMessage()
        {
            IsDeleted = false;
            IsSeen = false;
        }
    }
}
