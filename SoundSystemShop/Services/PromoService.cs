using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Powells.CouponCode;
using SoundSystemShop.Models;
using Options = Powells.CouponCode.Options;

namespace SoundSystemShop.Services
{
    public class PromoService
    {
        private readonly UserManager<AppUser> _userManager;

        public PromoService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public List<PromoVM> GenerateLuckyPersons(List<AppUser> users, int count = 3)
        {
            List<PromoVM> luckyUsers = new List<PromoVM>();
            Random random = new Random();
            HashSet<int> selectedIndices = new HashSet<int>();

            while (luckyUsers.Count < count && selectedIndices.Count < users.Count)
            {
                int userNumber = random.Next(0, users.Count);
                PromoVM promoVM = new();

                if (!selectedIndices.Contains(userNumber))
                {
                    string promoCode = GeneratePromoCode();
                    selectedIndices.Add(userNumber);
                    luckyUsers.Add(new PromoVM { User = users[userNumber], PromoCode = promoCode });
                }
            }

            return luckyUsers;
        }


        public string GeneratePromoCode()
        {
            var opts = new Options();
            var ccb = new CouponCodeBuilder();
            var badWords = ccb.BadWordsList;
            var code = ccb.Generate(opts);

            return code;
        }
    }

    public class PromoVM
    {
        public AppUser User { get; set; }
        public string PromoCode { get; set; }
    }
}
