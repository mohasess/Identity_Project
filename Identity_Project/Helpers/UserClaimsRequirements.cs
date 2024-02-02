using Identity_Project.Areas.Admin.Models.DTOs.BlogDTOs;
using Identity_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Identity_Project.Helpers
{
    public class UserClaimsRequirements : IAuthorizationRequirement
    {
        public int Age { get; set; }
        public UserClaimsRequirements(int age)
        {
            Age = age;
        }
        public UserClaimsRequirements()
        {

        }
    }


    public class UserAgeHandler : AuthorizationHandler<UserClaimsRequirements>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserClaimsRequirements requirement)
        {
            var claim = context.User.Claims.Where(c => c.Type == "Age").FirstOrDefault();
            if (claim != null)
            {
                var age = int.Parse(claim.Value);
                if (age >= requirement.Age)
                {
                    context.Succeed(requirement);
                }
                else
                    context.Fail(new AuthorizationFailureReason(this, "User is under age!"));
            }
            else
                context.Fail(new AuthorizationFailureReason(this, "Claim not found!"));

            return Task.CompletedTask;
        }
    }

    
    public class UserBlogHandler : AuthorizationHandler<UserClaimsRequirements, Blog>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserClaimsRequirements requirement, Blog resource)
        {
            if (context.User.Identity?.Name == resource.User.UserName)
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }
    }

    // for authorize in index view: cause we using BlogListDTO in index NOT the model "Blog"
    public class UserBlogListDTOHandler : AuthorizationHandler<UserClaimsRequirements, BlogListDTO>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserClaimsRequirements requirement, BlogListDTO resource)
        {
            if (context.User.Identity?.Name == resource.UserName)
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }
    }
}
