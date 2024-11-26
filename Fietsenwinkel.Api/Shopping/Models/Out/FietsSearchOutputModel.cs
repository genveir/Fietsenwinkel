using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Shopping.Models.Out;

public class FietsSearchOutputModel
{
    [JsonPropertyName("brandAndType")]
    public string FietsType { get; }

    [JsonPropertyName("numberOfWheels")]
    public int AantalWielen { get; }

    [JsonPropertyName("frameSize")]
    public int FrameSize { get; }

    [JsonPropertyName("price")]
    public int Price { get; }

    [JsonPropertyName("store")]
    public string FiliaalName { get; }

    public FietsSearchOutputModel(string fietsType, int aantalWielen, int frameSize, int price, string filiaalName)
    {
        FietsType = fietsType;
        AantalWielen = aantalWielen;
        FrameSize = frameSize;
        Price = price;
        FiliaalName = filiaalName;
    }
}