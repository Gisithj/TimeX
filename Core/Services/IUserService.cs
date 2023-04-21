using Microsoft.AspNetCore.Identity;
using TimeX.DTO.User;
using TimeX.Models;

namespace TimeX.Core.Services
{
    public interface IUserService
    {
        public Task<IdentityUser> CreateUser(IdentityUser user);
        public Task<IdentityUser> AssignRole(IdentityUser user, string roleName);
        string GetUserName();
        public string GetBusinessId();
        public string GetAdminId();
        public string GetCustomerId();


    }
}
