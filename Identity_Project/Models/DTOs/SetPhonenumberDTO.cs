using System.ComponentModel.DataAnnotations;

namespace Identity_Project.Models.DTOs
{
    public class SetPhonenumberDTO
    {
        [Required]
        [RegularExpression("^09\\d{9}$")]
        public string Phonenumber { get; set; }
    }
}
