using Elfie.Serialization;
using Identity_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity_Project.Helpers
{
    public class PasswordValidator : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string? password)
        {
            var file = "C:\\Users\\Stupid hobbit\\source\\repos\\Identity_Project\\Identity_Project\\Helpers\\WorstPasswordList.txt";
            var passArray = File.ReadAllLines(file);
            List<string> badPasswords = new List<string>(passArray);
            if (badPasswords.Contains(password))
            {
                var result = IdentityResult.Failed(new IdentityError()
                {
                    Code = "WorsePassword",
                    Description = "رمز عبور انتخابی شما ضعیف بوده و لطفا رمز دیگری انتخاب نمایید"
                });
                return Task.FromResult(result);
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
