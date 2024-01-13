using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Identity_Project.Models.DTOs
{
    public class ResetPasswordDTO
    {
        public string Id { get; set; }
        public string token { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
