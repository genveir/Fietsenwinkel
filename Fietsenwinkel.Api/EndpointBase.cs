using Fietsenwinkel.Domain.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api;

public class EndpointBase : ControllerBase
{
    protected IActionResult FormatError(ErrorCodeSet errorSet)
    {
        var errors = errorSet.Errors;

        var maxCode = errors.Select(e => (int)e).Max();
        var errorString = string.Join(", ", errors.Select(e => e.ToString()));

        return maxCode switch
        {
            < 1000 => NotFound(errorString),
            < 10000 => BadRequest(errorString),
            _ => StatusCode(500, errorString),
        };
    }

    protected Task<IActionResult> FormatErrorAsync(ErrorCodeSet errorSet) =>
        Task.FromResult(FormatError(errorSet));
}