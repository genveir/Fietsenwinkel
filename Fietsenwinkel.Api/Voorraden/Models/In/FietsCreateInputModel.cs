using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.In;

public class FietsCreateInputModel
{
    [JsonPropertyName("brandAndType")]
    public string? FietsType { get; set; }

    [JsonPropertyName("numberOfWheels")]
    public int? AantalWielen { get; set; }

    [JsonPropertyName("frameSize")]
    public int? FrameMaat { get; set; }

    [JsonPropertyName("price")]
    public int? Price { get; set; }
}
