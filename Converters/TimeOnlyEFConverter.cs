using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TimeX.Converters
{
    public class TimeOnlyEFConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyEFConverter() : base(
             t => t.ToTimeSpan(),
             ts => TimeOnly.FromTimeSpan(ts))
        { }
    }
}
