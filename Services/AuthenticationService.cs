using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TimeX.Core.Services;
using TimeX.Models;

namespace TimeX.Services
{
    public class AuthenticationService : IAuthenticationService

    {
        private readonly IConfiguration _configuration;
        public AuthenticationService(IConfiguration configuration) 
        {
            _configuration= configuration;
        }

        public  string CreateTokenAsync(Admin admin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim("AdminId",admin.AdminId.ToString()),
                new Claim(ClaimTypes.Role,"Admin")

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("Token:Key").Value
                ));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }
        public string CreateTokenAsync(Business business)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, business.Username),
                new Claim("BusinessId",business.BusinessId.ToString()),
                new Claim(ClaimTypes.Role,"Business")

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("Token:Key").Value
                ));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }public string CreateTokenAsync(Customer customer)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Username),
                new Claim("CustomerId",customer.CustomerId.ToString()),
                new Claim(ClaimTypes.Role,"Customer")

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("Token:Key").Value
                ));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }
    }
}
