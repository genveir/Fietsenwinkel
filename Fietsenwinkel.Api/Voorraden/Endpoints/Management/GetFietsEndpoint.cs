using Fietsenwinkel.Api.Voorraden.Mappers;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Admin.Voorraden;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorraden.Endpoints.Management;

public class GetFietsEndpoint : AdminEndpointBase
{
    private readonly IGetFietsUseCase getFietsUseCase;

    public GetFietsEndpoint(IGetFietsUseCase getFietsUseCase)
    {
        this.getFietsUseCase = getFietsUseCase;
    }

    [HttpGet("shops/{shopId}/stock/{bikeId}")]
    public async Task<IActionResult> GetFiets(string shopId, string bikeId) =>
        await Result.Combine(
            FiliaalId.Parse(shopId),
            FietsId.Parse(bikeId))
        .Map(
            onSuccess: GetFiets,
            onFailure: FormatErrorAsync);

    private async Task<IActionResult> GetFiets(FiliaalId filiaalId, FietsId fietsId)
    {
        var result = await getFietsUseCase.GetFiets(filiaalId, fietsId);

        return result.Map(
            onSuccess: fiets => Ok(FietsMapper.Map(fiets)),
            onFailure: FormatError
        );
    }
}
