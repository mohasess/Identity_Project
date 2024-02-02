using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework;

namespace Identity_Project.Areas.Admin.Models.DTOs.BlogDTOs
{
    public class BlogEditDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime WrittenDate { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        public string UserName { get; set; }

    }
}
