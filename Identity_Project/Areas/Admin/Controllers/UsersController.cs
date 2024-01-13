using Identity_Project.Areas.Admin.Models.DTOs.UserDTOs;
using Identity_Project.Models;
using Identity_Project.Models.DTOs;
using Identity_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.Where(u => u.EmailConfirmed == true).Select(u => new UserListDTO()
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
                return RedirectToAction("Index","Users", new { area = "Admin"});

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
            var user = _userManager.FindByIdAsync(userEditDTO.Id).Result;
            //var userMapped = AutoMapperConfig.mapper.Map<UserEditDTO, User>(userEditDTO);
            user.Firstname = userEditDTO.Firstname;
            user.Lastname = userEditDTO.Lastname;
            user.Email = userEditDTO.Email;
            user.UserName = userEditDTO.Username;
            user.PhoneNumber = userEditDTO.Phonenumber;
            var result = _userManager.UpdateAsync(user).Result;
            var messages = "";
            if (result.Succeeded)
                return RedirectToAction("Index", "Users", new { area = "Admin" });

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
                    return RedirectToAction("Index", "Users", new { area = "Admin" });
            }
            TempData["DeleteFailed"] = "Delete faile - please try again!";
            return RedirectToAction("Index", "Users", new { area = "Admin"});
        }
        public IActionResult AddUserRole(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var roles = new List<SelectListItem>(
                _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }).ToList()
                );
            var addUserRole = new AddUserRoleDTO()
            {
                Id = user.Id,
                Fullname = string.Format($"{user.Firstname} {user.Lastname}"),
                Roles = roles
            };
            return View(addUserRole);
        }
        [HttpPost]
        public IActionResult AddUserRole(AddUserRoleDTO addUserRole)
        {
            var user = _userManager.FindByIdAsync(addUserRole.Id).Result;
            var result = _userManager.AddToRoleAsync(user, addUserRole.Role).Result;
            if (result.Succeeded)
                return RedirectToAction("UserRoles", "Users", new { user.Id, area = "Admin" });
            var errors = "";
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }
            TempData["Errors"] = errors;
            return View(addUserRole);
        }
        public IActionResult UserRoles(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            return View(new UserRolesDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Fullname = string.Format($"{user.Firstname} {user.Lastname}"),
                Roles = roles
            });
        }
    }
}
