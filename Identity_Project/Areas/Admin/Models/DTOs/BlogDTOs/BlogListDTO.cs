using Identity_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Identity_Project.Areas.Admin.Models.DTOs.BlogDTOs
{
    public class BlogListDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime WrittenDate { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

    }
}
