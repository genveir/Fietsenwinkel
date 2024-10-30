using Fietsenwinkel.UseCases.Voorbeeld;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Voorbeeld;

public class ResetDbEndpoint : EndpointBase
{
    private readonly IResetDatabaseUseCase resetDatabaseUseCase;

    public ResetDbEndpoint(IResetDatabaseUseCase resetDatabaseUseCase)
    {
        this.resetDatabaseUseCase = resetDatabaseUseCase;
    }

    [HttpPost("resetDb")]
    public async Task<IActionResult> ResetDatabase()
    {
        await resetDatabaseUseCase.Execute();

        return NoContent();
    }
}