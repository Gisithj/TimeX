﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeX.Models
{
    public class Facility
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

        public ICollection<Reservation> Reservations { get; set; }

        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}
