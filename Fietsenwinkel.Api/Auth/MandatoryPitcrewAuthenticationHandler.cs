using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Fietsenwinkel.Api.Auth;

public class MandatoryPitcrewAuthenticationOptions : AuthenticationSchemeOptions
{ }

public class MandatoryPitcrewAuthenticationHandler : AuthenticationHandler<MandatoryPitcrewAuthenticationOptions>
{
    private readonly IHttpContextAccessor contextAccessor;

    public MandatoryPitcrewAuthenticationHandler(
        IHttpContextAccessor contextAccessor,
        IOptionsMonitor<MandatoryPitcrewAuthenticationOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder) : base(options, loggerFactory, urlEncoder)
    {
        this.contextAccessor = contextAccessor;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var context = contextAccessor.HttpContext;
        if (context == null)
        {
            return Fail;
        }

        var authHeaders = context.Request.Headers["DeWaarheid"];
        if (StringValues.IsNullOrEmpty(authHeaders) || authHeaders.Count > 1)
        {
            return Fail;
        }

        var authHeader = authHeaders[0];
        if (string.IsNullOrWhiteSpace(authHeader))
        {
            return Fail;
        }

        if (authHeader == "Ben en Ronald zijn de beste bazen en ik ben tevreden met mijn huidige salaris")
        {
            return Succeed();
        }

        return Fail;
    }

    private Task<AuthenticateResult> Succeed()
    {
        Claim[] claims = [
            new Claim(ClaimTypes.Name, "Werknemer"),
            new Claim(ClaimTypes.Role, "Administrator"),
            new Claim(ClaimTypes.UserData, "Voldoende gehoorzaam")
        ];
        var identity = new ClaimsIdentity(claims, "MandatoryPitcrew");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private static Task<AuthenticateResult> Fail => Task.FromResult(AuthenticateResult.Fail("onvoldoende commitment aan ons glorieuze leiderschap om admin taken uit te voeren"));
}