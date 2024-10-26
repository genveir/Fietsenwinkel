using Fietsenwinkel.Api.Voorraden.Mappers;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.UseCases.Voorraden;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorraden.Endpoints;

public class VoorraadDetailsEndpoint : EndpointBase
{
    private readonly IGetVoorraadDetailsUseCase getVoorraadDetailsUseCase;

    public VoorraadDetailsEndpoint(IGetVoorraadDetailsUseCase getVoorraadDetailsUseCase)
    {
        this.getVoorraadDetailsUseCase = getVoorraadDetailsUseCase;
    }

    [HttpGet("filialen/{filiaalId}/voorraad/details")]
    public async Task<IActionResult> GetDetails(string filiaalId)
    {
        return await FiliaalId.Parse(filiaalId).Switch(
            onSuccess: GetDetails,
            onFailure: FormatErrorAsync);

        async Task<IActionResult> GetDetails(FiliaalId filiaalId)
        {
            var result = await getVoorraadDetailsUseCase.GetDetails(filiaalId);

            return result.Switch(
                onSuccess: voorraad => Ok(VoorraadDetailsMapper.Map(voorraad)),
                onFailure: FormatError);
        }
    }
}