using SoundSystemShop.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Stripe;
using SoundSystemShop.Helper;

namespace SoundSystemShop.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public EmailService(IConfiguration configuration, IFileService fileService)
        {
            _configuration = configuration;
            _fileService = fileService;
        }

        public void Send(string to, string subject, string body)
        {
            //create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Smtp:FromAddress").Value));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            //////send mail
            
            //using SmtpClient smtp = new SmtpClient();
            //smtp.Connect(_configuration.GetSection("Smtp:Server").Value, int.Parse(_configuration.GetSection("Smtp:Port").Value), MailKit.Security.SecureSocketOptions.StartTls);
            //smtp.Authenticate(_configuration.GetSection("Smtp:FromAddress").Value, _configuration.GetSection("Smtp:Password").Value);
            //smtp.Send(email);
            //smtp.Disconnect(true);
        }

        public void PrepareEmail(EmailMember emailMember) 
        {
            string body = string.Empty;
            emailMember.path = "wwwroot/template/verify.html";
            emailMember.subject = "Modified New Product";
            body = _fileService.ReadFile(emailMember.path, body);
            body = body.Replace("{{Welcome}}", "Let's take a look at my new product");
            body = body.Replace("{{Confirm Account}}", "");
            body = body.Replace("{SaleDesc}", emailMember.message);
            Send(emailMember.email, emailMember.subject, body);
        }
    }
}
