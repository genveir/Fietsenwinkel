using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Shopping.Models.In;

public class FietsSearchInputModel
{
    [JsonPropertyName("userHeight")]
    public int? UserHeight { get; set; }

    [JsonPropertyName("userLocation")]
    public string? UserLocation { get; set; }

    [JsonPropertyName("userBudget")]
    public int? UserBudget { get; set; }

    [JsonPropertyName("typePreference")]
    public string? FietsTypePreference { get; set; }
}