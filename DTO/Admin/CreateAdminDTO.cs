using TimeX.Models;

namespace TimeX.DTO.Admin
{
    public class CreateAdminDTO
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;



    }
}
