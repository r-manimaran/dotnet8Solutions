using System.Text.Json.Serialization;

namespace UsingRefit.Models;

public class Actor
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
