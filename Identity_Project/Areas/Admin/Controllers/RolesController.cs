using Identity_Project.Areas.Admin.Models.DTOs.RoleDTOs;
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
        public RolesController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
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
            var role = _roleManager.FindByIdAsync(id);
            return View(role);
        }
        public IActionResult Edit(RoleEditDTO roleEditDTO)
        {
            var role = AutoMapperConfig.mapper.Map<RoleEditDTO, Role>(roleEditDTO);
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
    }
}
