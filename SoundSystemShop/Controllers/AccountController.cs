using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Controllers;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.Services.Interfaces;
using SoundSystemShop.ViewModels;
using static QRCoder.PayloadGenerator;

namespace SoundSystemShop.Areas.AdminArea.Controllers;

public class AccountController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountService _accountService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(IAccountService accountService, UserManager<AppUser> userManager)
    {
        _accountService = accountService;
        _userManager = userManager;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
        {
            return View(registerVM);
        }

        UserRegistrationResult registrationResult = await _accountService.RegisterUser(registerVM);

        switch (registrationResult)
        {
            case UserRegistrationResult.Success:
                return RedirectToAction(nameof(VerifyEmail), new { Email = registerVM.Email });
            case UserRegistrationResult.Failed:
                ModelState.AddModelError("", MessageConstants.RegistrationFailed);
                break;
            default:
                break;
        }

        return View(registerVM);
    }
    public IActionResult VerifyEmail(string email)
    {
        ConfirmAccountVM confirmAccountVM = new ConfirmAccountVM();
        confirmAccountVM.Email = email;
        return View(confirmAccountVM);
    }
    public async Task<IActionResult> ConfirmEmail(ConfirmAccountVM confirmAccountVM)
    {
        bool success = await _accountService.ConfirmEmailAndSignIn(confirmAccountVM);
        if (success)
        {
            return RedirectToAction(nameof(Login));
        }
        else
        {
            return RedirectToAction(nameof(VerifyEmail), new { Email = confirmAccountVM.Email });
        }
    }
    public async Task<IActionResult> RecendOTP(string email)
    {
        bool success = await _accountService.ResendOTP(email);
        if (success)
        {
            return RedirectToAction(nameof(VerifyEmail), new { Email = email });
        }
        else
        {
            return NotFound();
        }

    }
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
    {
        if (!ModelState.IsValid) return View();
        IdentityResult result = await _accountService.ChangePassword(User.Identity.Name, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);

        if (result.Succeeded)
        {
            ViewBag.IsSuccess = true;
            return View(changePasswordVM);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(changePasswordVM);
        }
    }
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgetPasswordVM forgetPasswordVM)
    {
        if (!ModelState.IsValid)
        {
            return View(forgetPasswordVM);
        }
        AppUser appUser = _accountService.GetUserByNameOrEmail(forgetPasswordVM.Email).Result;
        string token = _accountService.GeneratePasswordResetToken(appUser).Result;
        string resetLink = Url.Action(nameof(ResetPassword), "Account", new { userId = appUser.Id, token = token }, Request.Scheme, Request.Host.ToString());
        bool success = await _accountService.InitiatePasswordReset(forgetPasswordVM.Email, resetLink);

        if (success)
        {
            return RedirectToAction(nameof(ResetPasswordVerifyEmail));
        }
        else
        {
            ModelState.AddModelError("Email", "User not found");
            return View(forgetPasswordVM);
        }
    }
    public IActionResult ResetPasswordVerifyEmail()
    {
        return View();
    }
    public IActionResult ResetPassword(string userId, string token)
    {
        return View(new ResetPasswordVM { Token = token, UserId = userId });
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
    {
        if (!ModelState.IsValid) return View(resetPasswordVM);

        IdentityResult result = await _accountService.ResetPassword(resetPasswordVM);

        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Login));
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(resetPasswordVM);
        }

    }
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(loginVM);
        }

        LoginResult loginResult = await _accountService.Login(loginVM);

        switch (loginResult)
        {
            case LoginResult.Success:
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                bool isAdmin = await _accountService.GetRoleList(loginVM.UsernameOrEmail);
                if (isAdmin)
                {
                    return RedirectToAction(nameof(Index), "Dashboard", new { area = "adminarea" });
                }

                return RedirectToAction(nameof(Index), "Home");

            case LoginResult.UserNotFound:
                ModelState.AddModelError("", MessageConstants.UserNameOrPasswordIncorrect);
                break;

            case LoginResult.UserLockedOut:
                ModelState.AddModelError("", MessageConstants.AccountBlocked);
                break;

            case LoginResult.InvalidCredentials:
                ModelState.AddModelError("", MessageConstants.UserNameOrPasswordIncorrect);
                break;
        }

        return View(loginVM);
    }
    public async Task LoginWithGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("GoogleResponse")
        });
    }
    [Route("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var claims = result.Principal.Identities
            .FirstOrDefault().Claims.Select(claim => new
            {
                claim.Value,
                claim.Type,
                claim.Issuer,
                claim.OriginalIssuer
            });
        return Json(claims);
    }
    [Route("google-login")]
    public async Task<IActionResult> GoogleLogin()
    {
        var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse")};

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> Logout()
    {
        await _accountService.Logout();
        return RedirectToAction("Login");
    }

    public IActionResult GetUser(string userName)
    {
        if (string.IsNullOrEmpty(userName)) return null;
        var exist = _accountService.GetAllUsers().Where(p => p.UserName.ToLower().Contains(userName.ToLower()))
                .Take(3)
                .OrderByDescending(p => p.Id).ToList();
        return Json(exist);
    }

}