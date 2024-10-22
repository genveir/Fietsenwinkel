using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.Out;
internal class VoorraadListOutputModel
{
    [JsonPropertyName("stock")]
    public VoorraadListEntryOutputModel[] Voorraad { get; }

    public VoorraadListOutputModel(VoorraadListEntryOutputModel[] voorraad)
    {
        Voorraad = voorraad;
    }
}
