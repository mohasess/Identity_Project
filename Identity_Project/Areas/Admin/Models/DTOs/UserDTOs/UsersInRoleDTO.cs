using Identity_Project.Models;

namespace Identity_Project.Areas.Admin.Models.DTOs.UserDTOs
{
    public class UsersInRoleDTO
    {
        public string RoleName { get; set; }
        public IList<UserListDTO> Users { get; set; }
    }
}
