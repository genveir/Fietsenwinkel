using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.Out;
internal class VoorraadListEntryOutputModel
{
    [JsonPropertyName("brandAndType")]
    public string FietsType { get; }

    [JsonPropertyName("number")]
    public int Aantal { get; }

    public VoorraadListEntryOutputModel(string fietsType, int aantal)
    {
        FietsType = fietsType;
        Aantal = aantal;
    }
}
