using Identity_Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace Identity_Project.Helpers
{
    // Doesn't add claim to database !!!!!!
    // Uses for temporary and dynamics claims like "Shopping cart" and "User age"
    public class AddCustomClaim : UserClaimsPrincipalFactory<User>
    {
        public AddCustomClaim(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) 
        : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FullName",$"{user.Firstname} {user.Lastname}", ClaimValueTypes.String));
            identity.AddClaim(new Claim("LoginDate", DateTime.Now.ToString(), ClaimValueTypes.String));
            return identity;
        }
    }


}
