﻿using Fietsenwinkel.Api.Voorraden.Mappers;
using Fietsenwinkel.UseCases.Voorraden;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorraden.Endpoints;

public class ListFilialenEndpoint : EndpointBase
{
    private readonly IListFilialenUseCase listFilialenUseCase;

    public ListFilialenEndpoint(IListFilialenUseCase listFilialenUseCase)
    {
        this.listFilialenUseCase = listFilialenUseCase;
    }

    [HttpGet("shops")]
    public async Task<IActionResult> ListFilialen()
    {
        var result = await listFilialenUseCase.ListFilialen();

        return result.Map(
            onSuccess: filialen => Ok(FiliaaListlMapper.Map(filialen)),
            onFailure: FormatError);
    }
}