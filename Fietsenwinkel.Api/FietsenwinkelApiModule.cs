using Fietsenwinkel.Api.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.Api;

public static class FietsenwinkelApiModule
{
    public static void RegisterFietsenwinkelApiModule(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Helaas is deze authenticatie verplicht voor alle software die bij Pitcrew gedemonstreerd wordt.
        services.AddAuthentication()
            .AddScheme<MandatoryPitcrewAuthenticationOptions, MandatoryPitcrewAuthenticationHandler>(
                "MandatoryPitcrewAuth",
                options => { });
    }
}