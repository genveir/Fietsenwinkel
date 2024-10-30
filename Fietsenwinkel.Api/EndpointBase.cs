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

        if (errors.Select(e => (int)e).Max() < 10000)
        {
            return BadRequest(errors);
        }
        else
        {
            return StatusCode(500, errors);
        }
    }

    protected Task<IActionResult> FormatErrorAsync(ErrorCodeSet errorSet) =>
        Task.FromResult(FormatError(errorSet));
}