using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeX.DTO.Facility
{
    public class GetFacilityDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FacilityId { get; set; }
        [Required]
        public string FacilityName { get; set; } = string.Empty;
        [Required]
        public DayOfWeek BusinessDays { get; set; }
        [Required]
        public TimeOnly OpenTime { get; set; }
        [Required]
        public TimeOnly CloseTime { get; set; }
        [Required]
        public int SlotLength { get; set; }
        [Required]
        public int PerHour { get; set; }

        public int BusinessId { get; set; }
    }
}
