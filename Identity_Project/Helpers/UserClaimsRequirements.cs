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
}
