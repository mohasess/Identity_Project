using Identity_Project.Models;

namespace Identity_Project.Areas.Admin.Models.DTOs.UserDTOs
{
    public class UserRolesDTO
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
