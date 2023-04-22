namespace TimeX.DTO.Reservation
{
    public class GetReservationDto
    {
        public int Id { get; set; }
        public DateOnly date { get; set; }
        public int Amount { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int CustomerId { get; set; }
        public int FacilityId { get; set; }
    }
}
