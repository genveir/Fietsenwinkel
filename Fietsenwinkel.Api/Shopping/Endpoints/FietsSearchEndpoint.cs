using Fietsenwinkel.Api.Shopping.Mappers;
using Fietsenwinkel.Api.Shopping.Models.In;
using Fietsenwinkel.UseCases.Shopping;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Shopping.Endpoints;

public class FietsSearchEndpoint : EndpointBase
{
    private readonly IFietsSearchUseCase fietsSearchUseCase;

    public FietsSearchEndpoint(IFietsSearchUseCase fietsSearchUseCase)
    {
        this.fietsSearchUseCase = fietsSearchUseCase;
    }

    [HttpPost("fiets/search")]
    public async Task<IActionResult> FindFietsForUser([FromBody] FietsSearchInputModel fietsSearchInputModel)
    {
        return await FietsSearchMapper
            .Map(fietsSearchInputModel).Map(
                onSuccess: FindFietsForUser,
                onFailure: FormatErrorAsync);

        async Task<IActionResult> FindFietsForUser(FietsSearchQuery query)
        {
            var result = await fietsSearchUseCase.Search(query);

            return result.Map(
                onSuccess: fafn => Ok(FietsSearchMapper.Map(fafn)),
                onFailure: FormatError);
        }
    }
}