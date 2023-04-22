using System.ComponentModel.DataAnnotations;
using TimeX.Models;

namespace TimeX.Core.Services
{
    public interface IReservationService
    {
        public Task<bool> ValidateTimeSlot(Reservation reservation);
        public Task<int?> GetFacilityBusiness(int facilityId);


    }
}
