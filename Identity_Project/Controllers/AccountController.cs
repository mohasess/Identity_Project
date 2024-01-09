using Identity_Project.Models;
using Identity_Project.Models.DTOs;
using Identity_Project.Services;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterDTO registerDTO) 
        {
            //var user = new User()
            //{
            //    Firstname = registerDTO.Firstname,
            //    Lastname = registerDTO.Lastname,
            //    Email = registerDTO.Email,
            //    UserName = registerDTO.Email
            //};
            var user = AutoMapperConfig.mapper.Map<RegisterDTO, User>(registerDTO);
            user.UserName = registerDTO.Email;
            var result = _userManager.CreateAsync(user,registerDTO.Password).Result;
            var messages = "";
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            foreach (var error in result.Errors)
            {
                messages += error.Description + Environment.NewLine;
            }

            TempData["Messages"] = messages;
            return View(registerDTO);
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
    }
}
