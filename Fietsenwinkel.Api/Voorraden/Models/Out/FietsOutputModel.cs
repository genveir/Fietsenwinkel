using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.Out;

public class FietsOutputModel
{
    [JsonPropertyName("id")]
    public int FietsId { get; }

    [JsonPropertyName("brandAndType")]
    public string FietsType { get; }

    [JsonPropertyName("numberOfWheels")]
    public int AantalWielen { get; }

    [JsonPropertyName("frameSize")]
    public int FrameSize { get; }

    [JsonPropertyName("price")]
    public int Price { get; }

    public FietsOutputModel(int id, string fietsType, int aantalWielen, int frameSize, int price)
    {
        FietsId = id;
        FietsType = fietsType;
        AantalWielen = aantalWielen;
        FrameSize = frameSize;
        Price = price;
    }
}