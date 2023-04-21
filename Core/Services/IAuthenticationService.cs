using TimeX.Models;

namespace TimeX.Core.Services
{

    public interface IAuthenticationService
    {
        string CreateTokenAsync(Admin admin);
        string CreateTokenAsync(Business business);
        string CreateTokenAsync(Customer customer);

    }
}
