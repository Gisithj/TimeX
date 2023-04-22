using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TimeX.Core.Services;
using TimeX.Data;
using TimeX.Models;

namespace TimeX.Services
{
    public class ReservationService: IReservationService
    {
        private readonly TimeXDbContext _context;

        public ReservationService(TimeXDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ValidateTimeSlot(Reservation reservation)
        {
            if (reservation.StartTime >= reservation.EndTime)
            {
                return false;
            }

            var overlappingReservations = await _context.Reservation
                .Where(r => r.FacilityId == reservation.FacilityId &&
                            r.StartTime < reservation.EndTime &&
                            r.EndTime > reservation.StartTime &&
                            r.Id != reservation.Id) // exclude this reservation when checking for overlaps
                .ToListAsync();

            if (overlappingReservations.Any())
            {
                return false;
            }

            return true;
        }
        public async Task<int?> GetFacilityBusiness(int facilityId)
        {
            var businessId = await _context.Facility
                .Where(r => r.FacilityId == facilityId)
                .Select(r => r.BusinessId)
                .FirstOrDefaultAsync();

            return businessId;
        }

        
    }
}
