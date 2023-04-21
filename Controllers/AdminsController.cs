using System;
using System.Collections.Generic;
using System.Data;
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
using TimeX.DTO.User;
using TimeX.Models;
using TimeX.Security;

namespace TimeX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly TimeXDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _hasher;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AdminsController(
            TimeXDbContext context,
            IMapper mapper, 
            IPasswordHasher hasher,
            IUserService userService,
            IRoleService roleService)
        {
            _context = context;
            _mapper = mapper;
            _hasher = hasher;
            _userService = userService;
            _roleService = roleService;
        }

        // GET: api/Admins
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetAdminDTO>>> GetAdmin()
        {
            var admins = await _context.Admin.ToListAsync();
            return _mapper.Map<List<GetAdminDTO>>(admins);

        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAdminDTO>> GetAdmin(int id)
        {
            var admin = await _context.Admin.FindAsync(id);
            var returnAdmin = _mapper.Map<GetAdminDTO>(admin);

            if (admin == null)
            {
                return NotFound();
            }

            return returnAdmin;
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            if (id != admin.AdminId)
            {
                return BadRequest();
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetAdminDTO>> PostAdmin(CreateAdminDTO CreateAdminDTO)
        {
            try
            {
                //hash the user entered password
                CreateAdminDTO.PasswordHash =  _hasher.HashPassword(CreateAdminDTO.PasswordHash);
                //mapp the dto 
                var user = _mapper.Map<IdentityUser>(CreateAdminDTO);
                var newAdmin = _mapper.Map<Admin>(CreateAdminDTO);
                var response = _mapper.Map<GetAdminDTO>(newAdmin);

                //find the role
                var role = await _roleService.FindRoleByName("Admin");
                //assign the role and create the user
                if(role == null)
                {                    
                    return BadRequest("Role not found");
                }
                var result = await _userService.CreateUser(user);

                if (result !=null)
                {
                    await _userService.AssignRole(user, "Admin");
                    await _context.Admin.AddAsync(newAdmin);
                    await _context.SaveChangesAsync();
                }

                return response ;               

            }
            catch (Exception ex)
            {
                if (AdminUsernameExists(CreateAdminDTO.UserName))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Username already exists.");
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admin.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.AdminId == id);
        }
        private bool AdminUsernameExists(string username)
        {
            return _context.Admin.Any(e => e.Username == username);
        }
    }
}
