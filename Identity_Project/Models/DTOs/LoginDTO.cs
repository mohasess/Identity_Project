using System.ComponentModel.DataAnnotations;

namespace Identity_Project.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool IsPersistence { get; set; } = false;
        public string ReturnUrl { get; set; } 

    }
}
