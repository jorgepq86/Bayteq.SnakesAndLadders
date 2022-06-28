using System.Text.Json.Serialization;

namespace Bayteq.SnakesAndLadders.Domain.Entities;

public class BoardConfiguration
{
    [JsonPropertyName("specials")] public List<SpecialObject> SpecialObjects { get; set; }
    [JsonPropertyName("startPosition")] public int StartPosition { get; set; }
    [JsonPropertyName("lastPosition")] public int LastPosition { get; set; }
    
}