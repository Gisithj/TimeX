using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.DTO.Admin;
using TimeX.DTO.Business;
using TimeX.Models;
using TimeX.Security;
using TimeX.Services;

namespace TimeX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly TimeXDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _hasher;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public BusinessesController(TimeXDbContext context, IMapper mapper, IPasswordHasher hasher, IUserService userService,IRoleService roleService)
        {
            _context = context;
            _mapper = mapper;
            _hasher = hasher;
            _userService = userService;
            _roleService = roleService;
        }

        // GET: api/Businesses
/*        [Authorize(AuthenticationSchemes = "Bearer")]*/
        [HttpGet,Authorize(Roles = "Business")]

        public async Task<ActionResult<IEnumerable<Business>>> GetBusiness()
        {
            try
            {
                return await _context.Business.ToListAsync();

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Businesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> GetBusiness(int id)
        {
            var business = await _context.Business.FindAsync(id);

            if (business == null)
            {
                return NotFound();
            }

            return business;
        }

        // PUT: api/Businesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusiness(int id, Business business)
        {
            if (id != business.BusinessId)
            {
                return BadRequest();
            }

            _context.Entry(business).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(id))
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

        // POST: api/Businesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("RegisterBusiness")]
        public async Task<ActionResult<GetBusinessDTO>> RegisterBusiness(CreateBusinessDTO createBusinessDTO)
        {
            try
            {
                //hash the user entered password
                createBusinessDTO.PasswordHash = _hasher.HashPassword(createBusinessDTO.PasswordHash);
                //mapp the dto 
                var user = _mapper.Map<IdentityUser>(createBusinessDTO);
                var newBusiness = _mapper.Map<Business>(createBusinessDTO);
                var response = _mapper.Map<GetBusinessDTO>(newBusiness);

                //find the role
                var role = await _roleService.FindRoleByName("Business");
                //assign the role and create the user
                if (role == null)
                {
                    return BadRequest("Role not found");
                }
                var result = await _userService.CreateUser(user);

                if (result != null)
                {
                    await _userService.AssignRole(user, "Business");
                    await _context.Business.AddAsync(newBusiness);
                    await _context.SaveChangesAsync();
                }

                return response;

            }
            catch (Exception ex)
            {
                if (BusinessUsernameExists(createBusinessDTO.Username))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Username already exists.");
                }
                return BadRequest(ex.Message);
            }
           
           
        }

        // DELETE: api/Businesses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            var business = await _context.Business.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }

            _context.Business.Remove(business);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusinessExists(int id)
        {
            return _context.Business.Any(e => e.BusinessId == id);
        }

        private bool BusinessUsernameExists(string username)
        {
            return _context.Business.Any(e => e.Username == username);
        }

    }
}
