using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProductsApi.Converters
{
    public class BoolToIntConverter : ValueConverter<bool,int>
    {
        public BoolToIntConverter():base(
            v=>v?1:0,
            v=>v==1)
        {
            
        }
    }
}
