using Microsoft.AspNetCore.Identity;

namespace Identity_Project.Models
{
    public class User : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
