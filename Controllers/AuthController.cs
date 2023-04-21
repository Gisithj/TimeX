using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.DTO.Business;
using TimeX.Models;
using TimeX.Security;
using TimeX.Services;

namespace TimeX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TimeXDbContext _context;
        private readonly IPasswordHasher _hasher;
        private readonly IAdminService _adminServices;
        private  readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly IBusinessService _businessService;
        private readonly ICustomerService _customerService;

        public AuthController(
            TimeXDbContext context, 
            IAdminService adminServices,
            IPasswordHasher hasher,
            IConfiguration configuration,
            IAuthenticationService authenticationService,
            IBusinessService businessService,
            ICustomerService customerService
            )
        {
            _context = context;
            _hasher = hasher;
            _configuration = configuration;
            _authenticationService = authenticationService;
            _adminServices = adminServices;
            _businessService = businessService;
            _customerService = customerService;
        }

        [HttpPost("AdminLogin")]
       public async Task<ActionResult<string>> AdminLogin(LoginDto loginDto)
        {
          
            if(loginDto.username == null || loginDto.password == null)
            {
                return BadRequest("Username or password invalid");
            }
            var admin = await _adminServices.FindByUsername(loginDto.username);
            if (admin == null || !_hasher.PasswordMatches(loginDto.password, admin.PasswordHash))
            {
                return BadRequest("Username or Password Incorrect");
            }
            var token = _authenticationService.CreateTokenAsync(admin);
           // return Ok(token);
            return Ok(token);

        }
        [HttpPost("BusinessLogin")]
        public async Task<ActionResult<string>> BusinessLogin(LoginDto loginDto)
        {

            if (loginDto.username == null || loginDto.password == null)
            {
                return BadRequest("Username or password invalid");
            }
            var business = await _businessService.FindByUsername(loginDto.username);
            if (business == null || !_hasher.PasswordMatches(loginDto.password, business.PasswordHash))
            {
                return BadRequest("Username or Password Incorrect");
            }
            var token = _authenticationService.CreateTokenAsync(business);
            // return Ok(token);
            return Ok(token);

        }
        [HttpPost("CustomerLogin")]

        public async Task<ActionResult<string>> CustomerLogin(LoginDto loginDto)
        {

            if (loginDto.username == null || loginDto.password == null)
            {
                return BadRequest("Username or password invalid");
            }
            var customer = await _customerService.FindByUsername(loginDto.username);
            if (customer == null || !_hasher.PasswordMatches(loginDto.password, customer.PasswordHash))
            {
                return BadRequest("Username or Password Incorrect");
            }
            var token = _authenticationService.CreateTokenAsync(customer);
            // return Ok(token);
            return Ok(token);

        }



    }
}
