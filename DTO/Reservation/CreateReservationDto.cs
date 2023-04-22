namespace TimeX.DTO.Reservation
{
    public class CreateReservationDto
    {
        public DateOnly date { get; set; }
        public int Amount { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int FacilityId { get; set; }


    }
}
