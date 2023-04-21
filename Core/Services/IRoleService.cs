using Microsoft.AspNetCore.Identity;
using TimeX.Models;

namespace TimeX.Core.Services
{
    public interface IRoleService
    {
        public Task<IdentityRole> FindRoleByName(string name);
    }
}
