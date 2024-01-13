using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Identity_Project.Models.DTOs
{
    public class ForgetPasswordDTO
    {
        [System.ComponentModel.DataAnnotations.Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
