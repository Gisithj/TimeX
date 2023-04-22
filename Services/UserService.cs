using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.DTO.User;
using TimeX.Models;

namespace TimeX.Services
{
    public class UserService:IUserService
    {
        private readonly TimeXDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            TimeXDbContext context,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public async Task<IdentityUser> AssignRole(IdentityUser user,string roleName)
        {
            Console.WriteLine("in the assignRole");
            var roleAssigned= await _userManager.AddToRoleAsync(user, roleName);

            Console.WriteLine("IN the role assigning");
            Console.WriteLine(roleAssigned.Succeeded);

            return user;
        }

        public async Task<IdentityUser> CreateUser(IdentityUser user)
        {
            /*await _context.User.AddAsync(user);*/
            Console.WriteLine("IN the user creating1");
            var newUser = await _userManager.CreateAsync(user);
            Console.WriteLine("IN the user creating2");
            Console.WriteLine(newUser.Succeeded);

            return user;
        }

        public string GetUserName()
        {
            var result = string.Empty;
            if(_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            }
            return result;
        }
        public string GetAdminId()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue("AdminId");
            }
            return result;
        }
        public string GetBusinessId()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue("BusinessId");
            }
            return result;
        }
        public string GetCustomerId()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue("CustomerId");
            }
            return result;
        }
    }
}
