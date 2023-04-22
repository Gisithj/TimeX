using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.DTO.Reservation;
using TimeX.Models;

namespace TimeX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly TimeXDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IReservationService _reservationService;

        public ReservationsController(TimeXDbContext context,IMapper mapper,IUserService userService,IReservationService reservationService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _reservationService = reservationService;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservation()
        {
          if (_context.Reservation == null)
          {
              return NotFound();
          }
            return await _context.Reservation.ToListAsync();
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
          if (_context.Reservation == null)
          {
              return NotFound();
          }
            var reservation = await _context.Reservation.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Customer")]
        public async Task<ActionResult<Reservation>> PostReservation(CreateReservationDto CreateReservationDto)
        {
            if (CreateReservationDto == null)
            {
                return BadRequest("Invalid reservation request");
            }

            var reservation = _mapper.Map<Reservation>(CreateReservationDto);

            if (_reservationService.ValidateTimeSlot(reservation).Result)
            {
                var customerId = Convert.ToInt32(_userService.GetCustomerId());
                var businessId = await _reservationService.GetFacilityBusiness(reservation.FacilityId);

                reservation.CustomerId = customerId;
            }
            else
            {
                return BadRequest("The time slot is already booked");
            }

            if (_context.Reservation == null)
          {
              return Problem("Entity set 'TimeXDbContext.Reservation'  is null.");
          }
            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            if (_context.Reservation == null)
            {
                return NotFound();
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
