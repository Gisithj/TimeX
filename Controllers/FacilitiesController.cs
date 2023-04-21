using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.DTO.Facility;
using TimeX.Models;

namespace TimeX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilitiesController : ControllerBase
    {
        private readonly TimeXDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public FacilitiesController(TimeXDbContext context,IMapper mapper,IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        // GET: api/Facilities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetFacilityDto>>> GetFacility()
        {
          if (_context.Facility == null)
          {
              return NotFound();
          }
            var facilities = await _context.Facility.ToListAsync();
            return _mapper.Map<List<GetFacilityDto>>(facilities);
        }

        // GET: api/Facilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Facility>> GetFacility(int id)
        {
          if (_context.Facility == null)
          {
              return NotFound();
          }
            var facility = await _context.Facility.FindAsync(id);

            if (facility == null)
            {
                return NotFound();
            }

            return facility;
        }

        // PUT: api/Facilities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacility(int id, GetFacilityDto facility)
        {
            if (id != facility.FacilityId)
            {
                return BadRequest();
            }

            _context.Entry(facility).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacilityExists(id))
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

        // POST: api/Facilities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost,Authorize(Roles="Business")]
        public async Task<ActionResult<Facility>> PostFacility(CreateFacilityDto createFacilityDto)
        {
          if (_context.Facility == null)
          {
              return Problem("Entity set 'TimeXDbContext.Facility'  is null.");
          }
            //mapp the dto to model
            Console.WriteLine(createFacilityDto.FacilityName);

            var facilty = _mapper.Map<Facility>(createFacilityDto);
            //get the businessId from the token
            var businessId = _userService.GetBusinessId();

            //Add the facility
            facilty.BusinessId = Convert.ToInt32(businessId);
            Console.WriteLine(businessId);
            _context.Facility.Add(facilty);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFacility", new { id = facilty.FacilityId }, facilty);
        }

        // DELETE: api/Facilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            if (_context.Facility == null)
            {
                return NotFound();
            }
            var facility = await _context.Facility.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }

            _context.Facility.Remove(facility);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacilityExists(int id)
        {
            return (_context.Facility?.Any(e => e.FacilityId == id)).GetValueOrDefault();
        }
    }
}
