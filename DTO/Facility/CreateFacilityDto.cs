    using System.ComponentModel.DataAnnotations.Schema;

    namespace TimeX.DTO.Facility
    {
        public class CreateFacilityDto
        {
            public string FacilityName { get; set; } = string.Empty;
            public DayOfWeek BusinessDays { get; set; }
            public TimeOnly OpenTime { get; set; }
            public TimeOnly CloseTime { get; set; }
            public int SlotLength { get; set; }
            public int PerHour { get; set; }
        }
    }
