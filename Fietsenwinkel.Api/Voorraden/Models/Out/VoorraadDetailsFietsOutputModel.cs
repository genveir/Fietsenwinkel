using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.Out;

public class VoorraadDetailsFietsOutputModel
{
    [JsonPropertyName("brandAndType")]
    public string FietsType { get; }

    [JsonPropertyName("numberOfWheels")]
    public int AantalWielen { get; }

    [JsonPropertyName("frameSize")]
    public int FrameSize { get; }

    public VoorraadDetailsFietsOutputModel(string fietsType, int aantalWielen, int frameSize)
    {
        FietsType = fietsType;
        AantalWielen = aantalWielen;
        FrameSize = frameSize;
    }
}