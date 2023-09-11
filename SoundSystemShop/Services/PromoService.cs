using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Powells.CouponCode;
using SoundSystemShop.DAL;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using System.Linq;
using Options = Powells.CouponCode.Options;

namespace SoundSystemShop.Services
{
    public class PromoService
    {
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public PromoService(IEmailService emailService, IFileService fileService, IServiceScopeFactory scopeFactory, UserManager<AppUser> userManeger, AppDbContext appDbContext)
        {
            _emailService = emailService;
            _fileService = fileService;
            _userManager = userManeger;
            _appDbContext = appDbContext;
        }
        public bool GetPromo(string promo)
        {
            if (string.IsNullOrEmpty(promo)) return false;
            var existPromo = _appDbContext.PromoCodes.FirstOrDefault(p => p.Name == promo);
            if (existPromo == null) return false;
            return true;
        }
        public void GenerateLuckyPeopleAsync()
        {
            var count = _appDbContext.AdminPromos.FirstOrDefault(p => p.Name == "WeeklyPromo").UserCount;
            var users = _userManager.Users.ToList();

            List<PromoVM> luckyUsers = new List<PromoVM>();
            Random random = new Random();
            HashSet<int> selectedIndices = new HashSet<int>();

            while (luckyUsers.Count < count && selectedIndices.Count < users.Count)
            {
                int userNumber = random.Next(0, users.Count);
                PromoVM promoVM = new PromoVM();
                if (!selectedIndices.Contains(userNumber))
                {
                    string promoCode = GeneratePromoCode();
                    selectedIndices.Add(userNumber);

                    promoVM.User = users[userNumber];
                    promoVM.PromoCode = promoCode;
                    luckyUsers.Add(promoVM);
                }
            }

            foreach (var luckyUser in luckyUsers)
            {
                SendMailToLuckyPerson(luckyUser);
            }

        }
        public string GeneratePromoCode()
        {
            var opts = new Options();
            var ccb = new CouponCodeBuilder();
            var badWords = ccb.BadWordsList;
            var code = ccb.Generate(opts);
            var finish = _appDbContext.AdminPromos.FirstOrDefault(p => p.Name == "WeeklyPromo").FinishTime;
            PromoCode promoCode = new PromoCode()
            {
                Name = code,
                FinishDate = DateTime.Now.AddDays(finish),
            };
            _appDbContext.PromoCodes.Add(promoCode);

            
            //_appDbContext.SaveChanges();
            return code;
        }
        public void SendMailToLuckyPerson(PromoVM promoVM)
        {
            string body = string.Empty;
            string path = "wwwroot/template/verify.html";
            string subject = "Get Promo code";
            body = _fileService.ReadFile(path, body);
            body = body.Replace("{{Confirm Account}}", promoVM.PromoCode);
            body = body.Replace("{{Welcome!}}", promoVM.User.Fullname);
            //_emailService.Send(promoVM.User.Email, subject, body); 
        }

    }

    public class PromoVM
    {
        public AppUser User { get; set; }
        public string PromoCode { get; set; }
    }
}
