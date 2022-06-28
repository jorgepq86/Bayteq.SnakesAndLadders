using System.Text.Json.Serialization;
using Bayteq.SnakesAndLadders.Domain.Enums;

namespace Bayteq.SnakesAndLadders.Domain.Entities;

public class SpecialObject
{
    [JsonPropertyName("type")] public SpecialType SpecialType { get; set; }
    [JsonPropertyName("startNumber")] public int StartNumber { get; set; }
    [JsonPropertyName("endNumber")] public int EndNumber { get; set; }
}