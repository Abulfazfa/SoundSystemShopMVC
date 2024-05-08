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

            using SmtpClient smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtp.Connect(_configuration.GetSection("Smtp:Server").Value, int.Parse(_configuration.GetSection("Smtp:Port").Value), MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("Smtp:FromAddress").Value, _configuration.GetSection("Smtp:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void PrepareEmail(EmailMember emailMember) 
        {
            string body = string.Empty;
            emailMember.path = "wwwroot/template/" + emailMember.path;

            try
            {
                switch (emailMember.path)
                {
                    case ("SaleEmail.html"):
                        body = body.Replace("{{Sale}}", emailMember.salePercent);
                        body = body.Replace("{SaleDesc}", emailMember.saleDesc);
                        body = body.Replace("{Time}", emailMember.time);
                        break;

                    case ("verify.html"):
                        body = body.Replace("{{Welcome}}", emailMember.subject);
                        body = body.Replace("{{Confirm Account}}", "");
                        body = body.Replace("{SaleDesc}", emailMember.saleDesc);
                        break;

                    default:
                        break;
                }
                body = _fileService.ReadFile(emailMember.path, body);
           
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Send(emailMember.email, emailMember.subject, body);
            }
           
        }
    }
}
