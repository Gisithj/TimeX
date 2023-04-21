using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.Models;

namespace TimeX.Services
{
    public class AdminServices:IAdminService
    {
        private readonly TimeXDbContext _context;
        public AdminServices(TimeXDbContext context) 
        {
            _context= context;
        }

        public async Task<Admin?> FindByUsername(string username) { 

            var user= await _context.Admin.FirstOrDefaultAsync(u=>u.Username== username);
            
            return user;
        }

    }
}
