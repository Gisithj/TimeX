using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.DTO.Admin;
using TimeX.DTO.Customer;
using TimeX.Models;

namespace TimeX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly TimeXDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CustomersController(
            TimeXDbContext context,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IRoleService roleService,
            IUserService userService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _roleService = roleService;
            _userService = userService;
            _mapper= mapper;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
          if (_context.Customer == null)
          {
              return NotFound();
          }
            return await _context.Customer.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
          if (_context.Customer == null)
          {
              return NotFound();
          }
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetCustomerDto>> PostCustomer(CreateCustomerDto CreateCustomerDto)
        {
            try
            {
                //hash the user entered password
                CreateCustomerDto.PasswordHash = _passwordHasher.HashPassword(CreateCustomerDto.PasswordHash);
                //mapp the dto 
                var user = _mapper.Map<IdentityUser>(CreateCustomerDto);
                var newCustomer = _mapper.Map<Customer>(CreateCustomerDto);
                var response = _mapper.Map<GetCustomerDto>(newCustomer);

                //find the role
                var role = await _roleService.FindRoleByName("Customer");
                //assign the role and create the user
                if (role == null)
                {
                    return BadRequest("Role not found");
                }
                var result = await _userService.CreateUser(user);

                if (result != null)
                {
                    await _userService.AssignRole(user, "Customer");
                    await _context.Customer.AddAsync(newCustomer);
                    await _context.SaveChangesAsync();
                }

                return response;

            }
            catch (Exception ex)
            {
                if (CustomerUsernameExists(CreateCustomerDto.Username))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Username already exists.");
                }
                return BadRequest(ex.Message);
            }
          /*  if (_context.Customer == null)
          {
              return Problem("Entity set 'TimeXDbContext.Customer'  is null.");
          }
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);*/
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customer == null)
            {
                return NotFound();
            }
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customer?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
        private bool CustomerUsernameExists(string username)
        {
            return _context.Customer.Any(e => e.Username == username);
        }
    }
}
