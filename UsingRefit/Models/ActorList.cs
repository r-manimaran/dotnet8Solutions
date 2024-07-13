using System.Text.Json.Serialization;

namespace UsingRefit.Models;

public class ActorList
{
    [JsonPropertyName("results")]
    public List<Actor> Actors { get; set; }
}
