using Identity_Project.Areas.Admin.Models.DTOs.UserDTOs;
using Identity_Project.Models;
using Identity_Project.Models.DTOs;
using Identity_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.Where(u => u.LockoutEnabled == false).Select(u => new UserListDTO()
            {
                Id = u.Id,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                Phonenumber = u.PhoneNumber,
                AccessFailedCount = u.AccessFailedCount,
                EmailConfirmed = u.EmailConfirmed,
                LockoutEnabled = u.LockoutEnabled,
            });

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RegisterDTO registerDTO)
        {
            var user = AutoMapperConfig.mapper.Map<RegisterDTO, User>(registerDTO);
            user.UserName = registerDTO.Email;
            var result = _userManager.CreateAsync(user, registerDTO.Password).Result;
            var messages = "";
            if (result.Succeeded)
                return RedirectToAction("Index", "UsersController", new { area = "Admin"});

            foreach (var error in result.Errors)
            {
                messages += error.Description + Environment.NewLine;
            }

            TempData["Messages"] = messages;
            return View(registerDTO);
        }
        public IActionResult Edit(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            return View(AutoMapperConfig.mapper.Map<User,UserEditDTO>(user));
        }
        [HttpPost]
        public IActionResult Edit(UserEditDTO userEditDTO)
        {
            var user = AutoMapperConfig.mapper.Map<UserEditDTO, User>(userEditDTO);
            var result = _userManager.UpdateAsync(user).Result;
            var messages = "";
            if (result.Succeeded)
                return RedirectToAction("Index", "UsersController", new { area = "Admin" });

            foreach (var error in result.Errors)
            {
                messages += error.Description + Environment.NewLine;
            }
            TempData["Errors"] = messages;
            return View(userEditDTO);
        }
        
        public IActionResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if(user != null)
            {
                var result = _userManager.DeleteAsync(user).Result;
                if (result.Succeeded)
                    return RedirectToAction("Index", "UsersController", new { area = "Admin" });
            }
            TempData["DeleteFailed"] = "Delete faile - please try again!";
            return RedirectToAction("Index", "UsersController", new { area = "Admin"});
        }
    }
}
