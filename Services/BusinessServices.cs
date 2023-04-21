using Microsoft.EntityFrameworkCore;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.Models;

namespace TimeX.Services
{
    public class BusinessServices : IBusinessService
    {
        public readonly TimeXDbContext _context;
        public BusinessServices(TimeXDbContext context)
        {
            _context= context;
        }
        public async Task<Business?> FindByUsername(string username)
        {

            var user = await _context.Business.FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }
    }
}
