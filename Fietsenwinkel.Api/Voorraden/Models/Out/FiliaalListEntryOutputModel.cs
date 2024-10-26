using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.Out;

public class FiliaalListEntryOutputModel
{
    [JsonPropertyName("id")]
    public int Id { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    public FiliaalListEntryOutputModel(int id, string name)
    {
        Id = id;
        Name = name;
    }
}