using System.ComponentModel.DataAnnotations.Schema;

namespace TimeX.Models
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateOnly date { get; set; }
        public int Amount { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set;}

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public Facility Facility { get; set; }
        public int FacilityId { get; set; }
         
    }
}
