namespace Identity_Project.Areas.Admin.Models.DTOs.UserDTOs
{
    public class UserEditDTO
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phonenumber { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
