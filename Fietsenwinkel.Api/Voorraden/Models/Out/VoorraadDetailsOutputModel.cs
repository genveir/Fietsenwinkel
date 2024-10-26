using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.Out;

public class VoorraadDetailsOutputModel
{
    [JsonPropertyName("filiaalId")]
    public int FiliaalId { get; }

    [JsonPropertyName("bikes")]
    public VoorraadDetailsFietsOutputModel[] VoorraadDetailsFietsOutputModels { get; }

    public VoorraadDetailsOutputModel(int filiaalId, VoorraadDetailsFietsOutputModel[] voorraadDetailsFietsOutputModels)
    {
        FiliaalId = filiaalId;
        VoorraadDetailsFietsOutputModels = voorraadDetailsFietsOutputModels;
    }
}