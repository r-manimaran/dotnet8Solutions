using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProductsApi.Converters
{
    public class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeUtcConverter() : base(
        // When saving to database
        v => v.Kind == DateTimeKind.Unspecified ? 
            DateTime.SpecifyKind(v, DateTimeKind.Utc) : 
            v.ToUniversalTime(),
        // When reading from database
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc).ToLocalTime())
        {

        }

    }
    
}
