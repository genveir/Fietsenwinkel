using Fietsenwinkel.Api.Voorraden.Mappers;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.UseCases.Voorraden;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorraden.Endpoints;

[ApiController]
public class ListVoorraadEndpoint : EndpointBase
{
    private readonly IListVoorraadUseCase listVoorraadUseCase;

    public ListVoorraadEndpoint(IListVoorraadUseCase listVoorraadUseCase)
    {
        this.listVoorraadUseCase = listVoorraadUseCase;
    }

    [HttpGet("filialen/{filiaalId}/voorraad")]
    public async Task<IActionResult> ListVoorraad(string filiaalId, [FromQuery] string? filter)
    {
        return await FiliaalId.Parse(filiaalId).Switch(
            onSuccess: parsedId => ListVoorraad(parsedId, filter),
            onFailure: FormatErrorAsync);

        async Task<IActionResult> ListVoorraad(FiliaalId filiaalId, string? filter)
        {
            var result = await listVoorraadUseCase.GetVoorraad(new(filiaalId, filter));

            return result.Switch(
                onSuccess: voorraad => Ok(VoorraadListMapper.Map(voorraad)),
                onFailure: FormatError);
        }
    }
}