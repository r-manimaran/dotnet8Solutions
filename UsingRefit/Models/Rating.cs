using System.Text.Json.Serialization;

namespace UsingRefit.Models;
public class Rating
{
    [JsonPropertyName("value")]
    public decimal Value { get; set; }
}
