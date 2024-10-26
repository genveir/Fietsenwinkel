using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.Out;

public class FiliaalListOutputModel
{
    [JsonPropertyName("filialen")]
    public FiliaalListEntryOutputModel[] Filialen { get; }

    public FiliaalListOutputModel(FiliaalListEntryOutputModel[] filialen)
    {
        Filialen = filialen;
    }
}