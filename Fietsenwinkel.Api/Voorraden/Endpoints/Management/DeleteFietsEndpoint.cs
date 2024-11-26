using Fietsenwinkel.Api.Voorraden.Mappers;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Management;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorraden.Endpoints.Management;

public class DeleteFietsEndpoint : AdminEndpointBase
{
    private readonly IDeleteFietsUseCase deleteFietsUseCase;

    public DeleteFietsEndpoint(IDeleteFietsUseCase deleteFietsUseCase)
    {
        this.deleteFietsUseCase = deleteFietsUseCase;
    }

    [HttpDelete("shops/{shopId}/stock/{bikeId}")]
    public async Task<IActionResult> DeleteFiets(string shopId, string bikeId)
    {
        return await Result.Combine(
            FietsInputMapper.ParseFiliaalId(shopId),
            FietsInputMapper.ParseFietsId(bikeId))
            .Map(
                onSuccess: DeleteFiets,
                onFailure: FormatErrorAsync);

    }

    private async Task<IActionResult> DeleteFiets(FiliaalId filiaalId, FietsId fietsId)
    {
        var deleteResult = await deleteFietsUseCase.DeleteFiets(filiaalId, fietsId);

        return deleteResult.Map(
            onSuccess: NoContent,
            onFailure: FormatError);
    }
}
