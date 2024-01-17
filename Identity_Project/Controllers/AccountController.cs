using Identity_Project.Models;
using Identity_Project.Models.DTOs;
using Identity_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;
        private readonly SmsService _smsService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = new EmailService();
            _smsService = new SmsService();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterDTO registerDTO) 
        {
            var user = AutoMapperConfig.mapper.Map<RegisterDTO, User>(registerDTO);
            user.UserName = registerDTO.Email;
            var result = _userManager.CreateAsync(user,registerDTO.Password).Result;
            var messages = "";
            if (result.Succeeded)
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                var combackUrl = Url.Action("ConfirmEmail", "Account", new { token = token, id = user.Id }, protocol: Request.Scheme);
                var body = string.Format($"از طریق لینک زیر حساب کاربری خود را فعال سازی کنید <br/> <a href={combackUrl}>لینک فعال سازی</a>");
                _emailService.SendEmail(user.Email, body, "تایید حساب کاربری");
                return RedirectToAction("DisplayEmail", "Account", new { email = user.Email});
            }
            foreach (var error in result.Errors)
            {
                messages += error.Description + Environment.NewLine;
            }

            TempData["Messages"] = messages;
            return View(registerDTO);
        }
        public IActionResult DisplayEmail(string email)
        {
            ViewBag.Email = email;
            return View();
        }
        public IActionResult ConfirmEmail(string token, string id)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = _userManager.FindByIdAsync(id).Result;
            var result = _userManager.ConfirmEmailAsync(user, token).Result;
            var errors = "";
            if (result.Succeeded)
            {
                TempData["ConfirmEmail"] = "اکانت شما با موفقیت فعال سازی شد";
                return RedirectToAction("Login", "Account");
            }
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine; 
            }
            TempData["Errors"] = errors;
            return View();
        }
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginDTO() { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            
            if (!ModelState.IsValid)
            {
                TempData["Errors"] = "Login failed - check the entries and try again please!";
                return View(loginDTO);
            }
            var user = _userManager.FindByNameAsync(loginDTO.Email).Result;
            // if user was already signed in :
            _signInManager.SignOutAsync();

            var result = _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, loginDTO.IsPersistence, true).Result;
            // OR     
            //var resilt2 = _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.IsPersistence, true);

            if (result.Succeeded)
            {
                return Redirect(loginDTO.ReturnUrl);
            }
            else
            {
                TempData["Errors"] = "Login failed - Email or password is incorrect!";
                return View(loginDTO);
            }
            // User will locked out if try many incompleted attempts
            if (result.IsLockedOut)
            {
                TempData["Errors"] = "Login failed - User is locked temporary!";
                return View(loginDTO);
            }
            if (result.IsNotAllowed)
            {
                TempData["Errors"] = "Login failed - User is not allowed!";
                return View(loginDTO);
            }
            // Need two step verification
            if (result.RequiresTwoFactor)
            {
                //;
            }
            return View(loginDTO);
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            var user = _userManager.FindByEmailAsync(forgetPasswordDTO.Email.ToLower().Trim()).Result;
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var combackUrl = Url.Action("ResetPassword", "Account", new { token = token, id = user.Id }, protocol: Request.Scheme);
            var body = string.Format($"از طریق لینک زیر نسبت به تغییر رمز خود اقدام نمایید <br/>" +
                $" <a href={combackUrl}>لینک تغییر رمز کاربری</a>");
            _emailService.SendEmail(user.Email, body, "تغییر رمز کاربری");
            TempData["ResetPasswordEmailSent"] = "ایمیلی حاوی لینک تغییر رمز حساب کاربری برای شما ارسال شد";
            return View();
        }
        public IActionResult ResetPassword(string token, string id)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(id))
                return NotFound();
            return View(new ResetPasswordDTO()
            {
                Id = id,
                token = token,
            });
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDTO resetPassword)
        {
            var user = _userManager.FindByIdAsync(resetPassword.Id).Result;
            var result = _userManager.ResetPasswordAsync(user, resetPassword.token, resetPassword.Password).Result;
            if (result.Succeeded)
            {
                TempData["ResetPasswordDone"] = "تغییر رمز با موفقیت انجام شد!";
                return RedirectToAction("Login", "Account");
            }
            var errors = "";
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }
            TempData["Errors"] = errors;
            return View();
        }
        [Authorize]
        public IActionResult SetPhonenumber()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult SetPhonenumber(SetPhonenumberDTO setPhonenumberDTO)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var result = _userManager.SetPhoneNumberAsync(user, setPhonenumberDTO.Phonenumber).Result;
            var errors = "";
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }
            if (!result.Succeeded)
            {
                TempData["Errors"] = errors;
                return View();
            }
            var code = _userManager.GenerateChangePhoneNumberTokenAsync(user, setPhonenumberDTO.Phonenumber).Result;
            // api error : نیازمند سطح دسترسی بالا
            //_smsService.SendCustomSms(setPhonenumberDTO.Phonenumber, "اوقات خوشی رو براتون آرزومندیم بی ناموس");
            _smsService.SendSms(setPhonenumberDTO.Phonenumber, code);
            return RedirectToAction("VerifyPhonenumber", "Account", new { phonenumber = setPhonenumberDTO.Phonenumber });
        }
        [Authorize]
        public IActionResult VerifyPhonenumber(string phonenumber)
        {
            return View(new VerifyPhonenumberDTO()
            {
                Phonenumber = phonenumber
            });
        }
        [Authorize]
        [HttpPost]
        public IActionResult VerifyPhonenumber(VerifyPhonenumberDTO verifyPhonenumberDTO)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var verifyResult = _userManager.VerifyChangePhoneNumberTokenAsync(user, verifyPhonenumberDTO.Code, verifyPhonenumberDTO.Phonenumber).Result;
            if (!verifyResult)
            {
                TempData["Error"] = "کد وارد شده اشتباه است";
                return View(verifyPhonenumberDTO);
            }
            else
            {
                user.PhoneNumberConfirmed= true;
                _userManager.UpdateAsync(user);
                return View("VerifySuccess");
            }
        }
    }
}
