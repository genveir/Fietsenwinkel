using Fietsenwinkel.Api.Voorraden.Mappers;
using Fietsenwinkel.UseCases.Voorraden;
using Fietsenwinkel.UseCases.Voorraden.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorraden.Endpoints;

[ApiController]
public class ListVoorraadEndpoint : ControllerBase
{
    private readonly IListVoorraadUseCase listVoorraadUseCase;

    public ListVoorraadEndpoint(IListVoorraadUseCase listVoorraadUseCase)
    {
        this.listVoorraadUseCase = listVoorraadUseCase;
    }

    [HttpGet("voorraad")]
    public async Task<IActionResult> ListVoorraad([FromQuery] string? filter)
    {
        var result = await listVoorraadUseCase.GetVoorraad(new ListVoorraadQuery(filter));

        return result.Switch<IActionResult>(
            onSuccess: voorraad => Ok(VoorraadListMapper.Map(voorraad)),
            onFailure: error => BadRequest(error));
    }
}
