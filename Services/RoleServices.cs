using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeX.Core.Services;
using TimeX.Models;

namespace TimeX.Services
{
    public class RoleServices:IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleServices(RoleManager<IdentityRole> roleManager) 
        {
            _roleManager = roleManager;
        }
        public async Task<IdentityRole> FindRoleByName(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException("name");
            }
            var role = await _roleManager.FindByNameAsync(name);
            return role;
          
        }
    }
}
