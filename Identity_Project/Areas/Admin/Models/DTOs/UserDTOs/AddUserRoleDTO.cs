using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity_Project.Areas.Admin.Models.DTOs.UserDTOs
{
    public class AddUserRoleDTO
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
        [NotMapped]
        public List<SelectListItem> Roles { get; set; }
    }
}
