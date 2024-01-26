namespace Identity_Project.Areas.Admin.Models.DTOs.UserDTOs
{
    public class UserListDTO
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public int AccessFailedCount { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
