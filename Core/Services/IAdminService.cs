using TimeX.Models;

namespace TimeX.Core.Services
{
    public interface IAdminService
    {
        public Task<Admin?> FindByUsername(string username);
    }
}
