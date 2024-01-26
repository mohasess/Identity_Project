using Identity_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Identity_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    // Will check "saRolePolicy" before take access
    [Authorize(Policy = "saRolePolicy")]
    public class ClaimsController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ClaimsController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // Will check "VerifiedAgePolicy" before take 
        [Authorize(Policy = "VerifiedAgePolicy")]
        public IActionResult Index()
        {
            return View(User.Claims);
        }


        [Authorize(Policy = "VerifiedAgeByHandler")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(string claimType, string value)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var claim = new Claim(claimType, value, ClaimValueTypes.String);
            // Add claim to database!!!
            var result = _userManager.AddClaimAsync(user, claim).Result;
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }


        public IActionResult Delete(string claimType)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var claim = User.Claims.Where(c => c.Type == claimType).FirstOrDefault();
            if (claim != null)
            {
                var result = _userManager.RemoveClaimAsync(user,claim).Result;
                if (result.Succeeded)
                {
                    ViewData["DeleteStatus"] = "Claim has been deleted";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["DeleteStatus"] = "Delete failed!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return NotFound();
        }


    }
}
