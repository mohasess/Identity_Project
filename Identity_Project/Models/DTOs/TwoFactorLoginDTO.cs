using Microsoft.AspNetCore.Components.Web;
using Microsoft.Build.Framework;

namespace Identity_Project.Models.DTOs
{
    public class TwoFactorLoginDTO
    {
        [Required]
        public string Code { get; set; }
        public bool IsPersistence { get; set; }
        public string ReturnUrl { get; set; }
        public string Provider { get; set; }
    }
}
