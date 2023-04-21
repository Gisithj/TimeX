using TimeX.Models;

namespace TimeX.Core.Services
{
    public interface IBusinessService
    {
        public Task<Business?> FindByUsername(string username);
    }
}
