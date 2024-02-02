using Humanizer;
using Identity_Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity_Project.Helpers
{
    public class CustomClaimTransform : IClaimsTransformation
    {
        private readonly IAuthorizationService _authorizeService;
        private readonly UserManager<User> _userManager;
        public CustomClaimTransform(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // Dynamic claims : calculate age of user everytime he logins!!! 
        // Does not add his age to database!!!
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                if (user != null)
                {
                    // Age Claim

                    var age = ((int)(DateTime.Now.Subtract(user.BirthDate).Days / 365.25));
                    identity.AddClaim(new Claim("Age", age.ToString(), ClaimValueTypes.String));

                    // VerifiedAge Claim

                    if (age >= 18)
                    {
                        identity.AddClaim(new Claim("VerifiedAge", "true", ClaimValueTypes.String));
                    }
                }
            }
            return await Task.FromResult(principal);
        }
    }
}
