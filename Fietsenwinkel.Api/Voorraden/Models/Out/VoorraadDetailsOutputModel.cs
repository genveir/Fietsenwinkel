using System.Text.Json.Serialization;

namespace Fietsenwinkel.Api.Voorraden.Models.Out;

public class VoorraadDetailsOutputModel
{
    [JsonPropertyName("filiaalId")]
    public int FiliaalId { get; }

    [JsonPropertyName("bikes")]
    public FietsOutputModel[] VoorraadDetailsFietsOutputModels { get; }

    public VoorraadDetailsOutputModel(int filiaalId, FietsOutputModel[] voorraadDetailsFietsOutputModels)
    {
        FiliaalId = filiaalId;
        VoorraadDetailsFietsOutputModels = voorraadDetailsFietsOutputModels;
    }
}