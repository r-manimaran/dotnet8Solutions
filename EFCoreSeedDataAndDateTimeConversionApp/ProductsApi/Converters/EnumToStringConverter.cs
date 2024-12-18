using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProductsApi.Converters
{
    public class EnumToStringConverter<T> : ValueConverter<T, string> where T: Enum
    {
        public EnumToStringConverter():base(
            v=>v.ToString(),
            v=>(T)Enum.Parse(typeof(T),v))
        { }
    }

    public class EnumToIntConverter<T> : ValueConverter<T,int> where T: Enum
    {
        public EnumToIntConverter():base(
            v=>Convert.ToInt32(v),
            v=>(T)Enum.ToObject(typeof(T),v))
        {
            
        }
    }
}
