using Identity_Project.Areas.Admin.Models.DTOs.RoleDTOs;
using Identity_Project.Areas.Admin.Models.DTOs.UserDTOs;
using Identity_Project.Models;
using Identity_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(AutoMapperConfig.mapper.Map<List<Role>,List<RoleListDTO>>(roles));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RoleCreateDTO roleCreateDTO)
        {
            var role = AutoMapperConfig.mapper.Map<RoleCreateDTO, Role>(roleCreateDTO);
            var result = _roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
                return RedirectToAction("Index", "Roles", new { area = "Admin" });
            var errors = "";
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }
            TempData["Errors"] = errors;
            return View(roleCreateDTO);
        }
        public IActionResult Edit(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            return View(AutoMapperConfig.mapper.Map<Role,RoleEditDTO>(role));
        }
        [HttpPost]
        public IActionResult Edit(RoleEditDTO roleEditDTO)
        {
            //var role = AutoMapperConfig.mapper.Map<RoleEditDTO, Role>(roleEditDTO);
            var role = _roleManager.FindByIdAsync(roleEditDTO.Id).Result;
            role.Name = roleEditDTO.Name;
            role.Title = roleEditDTO.Title;
            var result = _roleManager.UpdateAsync(role).Result;
            if (result.Succeeded)
                return RedirectToAction("Index", "Roles", new { area = "Admin" });
            var errors = "";
            foreach (var error in result.Errors)
            {
                    errors += error.Description + Environment.NewLine;
            }
            TempData["Errors"] = errors;
            return View(roleEditDTO);
        }
        public IActionResult UsersInRole(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            var users = _userManager.GetUsersInRoleAsync(role.Name).Result;
            var usersDTO = AutoMapperConfig.mapper.Map<IList<User>, IList<UserListDTO>>(users);
            return View(new UsersInRoleDTO()
            {
                RoleName = role.Name,
                Users = usersDTO
            });

        }
    }
}
