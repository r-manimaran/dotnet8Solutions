using System.Text.Json.Serialization;

namespace UsingRefit.Models;

public class MovieList
{
    [JsonPropertyName("cast")]
    public List<Movie> Movies { get; set; }
}
