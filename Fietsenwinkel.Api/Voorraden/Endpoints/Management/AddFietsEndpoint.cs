using Fietsenwinkel.Api.Voorraden.Mappers;
using Fietsenwinkel.Api.Voorraden.Models.In;
using Fietsenwinkel.UseCases.Voorraden.Management;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorraden.Endpoints.Management;

public class AddFietsEndpoint : AdminEndpointBase
{
    private readonly IAddFietsUseCase addFietsUseCase;

    public AddFietsEndpoint(IAddFietsUseCase addFietsUseCase)
    {
        this.addFietsUseCase = addFietsUseCase;
    }

    [HttpPost("shops/{filiaalId}/stock")]
    public async Task<IActionResult> AddFiets(string filiaalId, [FromBody] FietsCreateInputModel fietsCreateModel)
    {
        return await FietsCreateMapper.Map(filiaalId, fietsCreateModel)
            .Map(
                onSuccess: AddFiets,
                onFailure: FormatErrorAsync);
    }

    private async Task<IActionResult> AddFiets(FietsCreateModel fietsCreateModel)
    {
        var result = await addFietsUseCase.Add(fietsCreateModel);

        return result.Map(
            onSuccess: fiets => Ok(FietsMapper.Map(fiets)),
            onFailure: FormatError);
    }
}
