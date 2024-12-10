using Fietsenwinkel.Api.Voorraden.Mappers;
using Fietsenwinkel.Api.Voorraden.Models.In;
using Fietsenwinkel.UseCases.Admin.Voorraden;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorraden.Endpoints.Management;

public class UpdateFietsEndpoint : AdminEndpointBase
{
    private readonly IUpdateFietsUseCase updateFietsUseCase;

    public UpdateFietsEndpoint(IUpdateFietsUseCase updateFietsUseCase)
    {
        this.updateFietsUseCase = updateFietsUseCase;
    }

    [HttpPut("shops/{shopId}/stock/{bikeId}")]
    public async Task<IActionResult> UpdateFiets(string shopId, string bikeId, [FromBody] FietsUpdateInputModel fietsUpdateModel) =>
        await FietsUpdateMapper.Map(shopId, bikeId, fietsUpdateModel)
            .Map(
                onSuccess: UpdateFiets,
                onFailure: FormatErrorAsync);

    private async Task<IActionResult> UpdateFiets(FietsUpdateModel fietsUpdateModel)
    {
        var result = await updateFietsUseCase.Update(fietsUpdateModel);

        return result.Map(
            onSuccess: fiets => Ok(FietsMapper.Map(fiets)),
            onFailure: FormatError
        );
    }
}
