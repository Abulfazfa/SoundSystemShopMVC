using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoundSystemShop.Helper;
using SoundSystemShop.Models;
using SoundSystemShop.Services;
using SoundSystemShop.ViewModels;

namespace SoundSystemShop.Areas.AdminArea.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly FileService _fileService;
    private readonly EmailService _emailService;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, FileService fileService, EmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _fileService = fileService;
        _emailService = emailService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid) return View();
        string otp = GenerateOTP();
        AppUser appUser = new()
        {
            Fullname = registerVM.Fullname,
            Email = registerVM.Email,
            UserName = registerVM.Username,
            OTP = otp
        };

        var result = await _userManager.CreateAsync(appUser, registerVM.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", errorMessage: error.Description);
            }
            return View(registerVM);
        }

        await _userManager.AddToRoleAsync(appUser, RoleEnum.User.ToString());
        string body = string.Empty;
        string path = "wwwroot/template/verify.html";
        string subject = "Verify Email";
        body = _fileService.ReadFile(path, body);
        body = body.Replace("{{Confirm Account}}", otp);
        body = body.Replace("{{Welcome!}}", appUser.Fullname);

        _emailService.Send(appUser.Email, subject, body);
        return RedirectToAction(nameof(VerifyEmail), new { Email = appUser.Email });

    }
    public IActionResult VerifyEmail(string email)
    {
        ConfirmAccountVM confirmAccountVM = new ConfirmAccountVM();
        confirmAccountVM.Email = email;
        return View(confirmAccountVM);
    }

    public async Task<IActionResult> ConfirmEmail(ConfirmAccountVM confirmAccountVM)
    {
        AppUser existUser = await _userManager.FindByEmailAsync(confirmAccountVM.Email);
        if (existUser == null) return NotFound();
        if (existUser.OTP != confirmAccountVM.OTP || string.IsNullOrEmpty(confirmAccountVM.OTP))
        {
            TempData["Error"] = "Wrong OTP";
            RedirectToAction(nameof(VerifyEmail), new { Email = confirmAccountVM.Email });
        }
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(existUser);
        await _userManager.ConfirmEmailAsync(existUser, token);
        await _signInManager.SignInAsync(existUser, isPersistent: false);
        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> RecendOTP(string email)
    {
        string otp = GenerateOTP();
        AppUser existUser = await _userManager.FindByEmailAsync(email);
        existUser.OTP = otp;
        await _userManager.UpdateAsync(existUser);

        string body = string.Empty;
        string path = "wwwroot/assets/templates/verify.html";
        string subject = "Verify Email";
        body = _fileService.ReadFile(path, body);
        body = body.Replace("{{Confirm Account}}", otp);
        body = body.Replace("{{Welcome!}}", existUser.Fullname);

        _emailService.Send(existUser.Email, subject, body);
        return RedirectToAction(nameof(VerifyEmail), new { Email = email });

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
        AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);
        IdentityResult result = await _userManager.ChangePasswordAsync(existUser, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);
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
        }
        return View(changePasswordVM);
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgetPasswordVM forgetPasswordVM)
    {
        if (!ModelState.IsValid) return View(forgetPasswordVM);
        AppUser existUser = await _userManager.FindByEmailAsync(forgetPasswordVM.Email);
        if (existUser == null)
        {
            ModelState.AddModelError("Email", "User not found ");
            return View(forgetPasswordVM);
        }
        string token = await _userManager.GeneratePasswordResetTokenAsync(existUser);
        string link = Url.Action(nameof(ResetPassword), "Account", new { userId = existUser.Id, token }, Request.Scheme, Request.Host.ToString());
        string body = string.Empty;
        string path = "wwwroot/template/ForgotPassword.html";
        string subject = "Verify Email";
        body = _fileService.ReadFile(path, body);
        body = body.Replace("{{link}}", link);
        body = body.Replace("{{Welcome!}}", existUser.Fullname);

        _emailService.Send(existUser.Email, subject, body);
        return RedirectToAction(nameof(ResetPasswordVerifyEmail));
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
        AppUser existUser = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
        if (existUser == null) return NotFound();

        if (await _userManager.CheckPasswordAsync(existUser, resetPasswordVM.Password))
        {
            ModelState.AddModelError("", "This password already");
            return View(resetPasswordVM);
        }
        await _userManager.ResetPasswordAsync(existUser, resetPasswordVM.Token, resetPasswordVM.Password);
        return RedirectToAction(nameof(Login));

    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
    {
        if (!ModelState.IsValid) return View();
        AppUser appUser = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
        if (appUser == null)
        {
            ModelState.AddModelError("", "username or password isn't true");
            return View(loginVM);
        }
        var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RememberMe, true);
        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "your profile has been blocked");
            return View(loginVM);
        }
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "username or password isn't true");
            return View(loginVM);
        }
        if (ReturnUrl != null)
        {
            return Redirect(ReturnUrl);
        }

        await _signInManager.SignInAsync(appUser, loginVM.RememberMe);
        var userList = await _userManager.GetRolesAsync(appUser);
        if (userList.Contains(RoleEnum.Admin.ToString())) return RedirectToAction("Index", "Dashboard", new { area = "adminarea" });

        return RedirectToAction("Index", "Home");



    }
    public async Task LoginWithGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("GoogleResponse")
        });
    }

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

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    public async Task<IActionResult> AddRole()
    {
        foreach (var item in Enum.GetValues(typeof(RoleEnum)))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
        }
        return Content("Roles added");
    }

    private static string GenerateOTP()
    {
        Random random = new();
        int otpNumber = random.Next(1000, 9999);
        return otpNumber.ToString();
    }

}