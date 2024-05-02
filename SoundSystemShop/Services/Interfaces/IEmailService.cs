using SoundSystemShop.Helper;

namespace SoundSystemShop.Services.Interfaces
{
    public interface IEmailService
    {
        public void Send(string to, string subject, string body);
        public void PrepareEmail(EmailMember emailMember);
    }
}
