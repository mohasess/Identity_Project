using System.ComponentModel.DataAnnotations;

namespace Identity_Project.Areas.Admin.Models.DTOs.BlogDTOs
{
    public class BlogCreateDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public DateTime WrittenDate { get; set; } = DateTime.Now;
    }
}
