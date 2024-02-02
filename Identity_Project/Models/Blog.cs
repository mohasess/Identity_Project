using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace Identity_Project.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime WrittenDate { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

    }
}
