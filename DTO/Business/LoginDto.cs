using Microsoft.Build.Framework;
namespace TimeX.DTO.Business
{
    public class LoginDto
    {
        [Required]
        public string username { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }
}
