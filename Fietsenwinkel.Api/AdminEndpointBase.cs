using Microsoft.AspNetCore.Authorization;

namespace Fietsenwinkel.Api;

[Authorize]
public class AdminEndpointBase : EndpointBase
{
}