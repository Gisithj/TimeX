using TimeX.Models;

namespace TimeX.Core.Services
{
    public interface ICustomerService
    {
        public Task<Customer?> FindByUsername(string username);

    }
}
