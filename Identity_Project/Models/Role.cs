using Microsoft.AspNetCore.Identity;

namespace Identity_Project.Models
{
    public class Role : IdentityRole
    {
        public string Title { get; set; }
    }
}
