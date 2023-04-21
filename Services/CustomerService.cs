using Microsoft.EntityFrameworkCore;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.Models;

namespace TimeX.Services
{
    public class CustomerService:ICustomerService
    {
        public readonly TimeXDbContext _context;
        public CustomerService(TimeXDbContext context)
        {
            _context = context;
        }
        public async Task<Customer?> FindByUsername(string username)
        {

            var user = await _context.Customer.FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }
    }
}
