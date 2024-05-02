using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Services
{
    public interface IAccountService
    {
        Task<UserRegistrationResult> RegisterUser(RegisterVM registerVM);
        Task<bool> ConfirmEmailAndSignIn(ConfirmAccountVM confirmAccountVM);
        Task<bool> ResendOTP(string email);
        Task<IdentityResult> ChangePassword(string userName, string currentPassword, string newPassword);
        Task<bool> InitiatePasswordReset(string email, string resetLink);
        Task<IdentityResult> ResetPassword(ResetPasswordVM resetPasswordVM);
        Task<LoginResult> Login(LoginVM loginVM);
        Task Logout();
        Task<bool> GetRoleList(string userNameOrEmail);
        Task<AppUser> GetUserByNameOrEmail(string userNameOrEmail);
        Task<string> GeneratePasswordResetToken(AppUser existUser);
        List<AppUser> GetAllUsers();
    }


    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IFileService fileService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _fileService = fileService;
            _emailService = emailService;
        }

        public async Task<UserRegistrationResult> RegisterUser(RegisterVM registerVM)
        {
            AppUser appUser = CreateAppUserFromViewModel(registerVM);
            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                return UserRegistrationResult.Failed;
            }

            await AssignUserRoleAsync(appUser);
            SendVerificationEmailAsync(appUser);

            return UserRegistrationResult.Success;
        }
        private AppUser CreateAppUserFromViewModel(RegisterVM registerVM)
        {
            return new AppUser
            {
                Fullname = registerVM.Fullname,
                Email = registerVM.Email,
                UserName = registerVM.Username,
                Location = registerVM.Location,
                OTP = GenerateOTP()
            };
        }
        private async Task AssignUserRoleAsync(AppUser appUser)
        {
            await _userManager.AddToRoleAsync(appUser, RoleEnum.User.ToString());
        }
        private void SendVerificationEmailAsync(AppUser appUser)
        {
            string body = string.Empty;
            string path = "wwwroot/template/verify.html";
            string subject = "Verify Email";
            body = _fileService.ReadFile(path, body);
            body = body.Replace("{{Confirm Account}}", appUser.OTP);
            body = body.Replace("{{Welcome}}", appUser.Fullname);
            body = body.Replace("{SaleDesc}", "");

            _emailService.Send(appUser.Email, subject, body);
        }
        public async Task<bool> ConfirmEmailAndSignIn(ConfirmAccountVM confirmAccountVM)
        {
            AppUser existUser = await _userManager.FindByEmailAsync(confirmAccountVM.Email);
            if (existUser == null)
            {
                return false; 
            }

            if (existUser.OTP != confirmAccountVM.OTP || string.IsNullOrEmpty(confirmAccountVM.OTP))
            {
                return false;
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(existUser);
            await _userManager.ConfirmEmailAsync(existUser, token);
            await _signInManager.SignInAsync(existUser, isPersistent: false);

            return true;
        }
        public async Task<bool> ResendOTP(string email)
        {
            string otp = GenerateOTP();
            AppUser existUser = await _userManager.FindByEmailAsync(email);

            if (existUser == null)
            {
                return false; // User not found, could not resend OTP.
            }

            existUser.OTP = otp;
            await _userManager.UpdateAsync(existUser);

            string body = string.Empty;
            string path = "wwwroot/assets/templates/verify.html";
            string subject = "Verify Email";
            body = _fileService.ReadFile(path, body);
            body = body.Replace("{{Confirm Account}}", otp);
            body = body.Replace("{{Welcome!}}", existUser.Fullname);

            _emailService.Send(existUser.Email, subject, body);
            return true;
        }
        public async Task<IdentityResult> ChangePassword(string userName, string currentPassword, string newPassword)
        {
            AppUser existUser = await _userManager.FindByNameAsync(userName);
            if (existUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(existUser, currentPassword, newPassword);
            return result;
        }
        public async Task<bool> InitiatePasswordReset(string email, string resetLink)
        {
            AppUser existUser = await _userManager.FindByEmailAsync(email);
            if (existUser == null)
            {
                return false; 
            }

            
            string link = resetLink;

            string body = string.Empty;
            string path = "wwwroot/template/ForgotPassword.html";
            string subject = "Password Reset Request";
            body = _fileService.ReadFile(path, body);
            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Welcome!}}", existUser.Fullname);
            body = body.Replace("{SaleDesc}", "");

            _emailService.Send(existUser.Email, subject, body);
            return true;
        }
        public async Task<IdentityResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            AppUser existUser = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (existUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            if (await _userManager.CheckPasswordAsync(existUser, resetPasswordVM.Password))
            {
                return IdentityResult.Failed(new IdentityError { Description = "The new password must be different from the existing password." });
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(existUser, resetPasswordVM.Token, resetPasswordVM.Password);
            return result;
        }
        public async Task<LoginResult> Login(LoginVM loginVM)
        {
            AppUser appUser = GetUserByNameOrEmail(loginVM.UsernameOrEmail).Result;
            if (appUser == null)
            {
                return LoginResult.UserNotFound;
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RememberMe, true);

            if (result.IsLockedOut)
            {
                return LoginResult.UserLockedOut;
            }

            if (!result.Succeeded)
            {
                return LoginResult.InvalidCredentials;
            }
            await _signInManager.SignInAsync(appUser, loginVM.RememberMe);
            return LoginResult.Success;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<bool> GetRoleList(string userNameOrEmail)
        {
            AppUser appUser = await GetUserByNameOrEmail(userNameOrEmail);
            var roles = await _userManager.GetRolesAsync(appUser);
            return roles.Contains(RoleEnum.Admin.ToString());
        }
        private static string GenerateOTP()
        {
            Random random = new();
            int otpNumber = random.Next(1000, 9999);
            return otpNumber.ToString();
        }
        public async Task<AppUser> GetUserByNameOrEmail(string userNameOrEmail)
        {
           return await _userManager.FindByNameAsync(userNameOrEmail) ?? await _userManager.FindByEmailAsync(userNameOrEmail);
        }
        public async Task<string> GeneratePasswordResetToken(AppUser existUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(existUser);
        }
        public List<AppUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }
    }


}
