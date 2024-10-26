using Fietsenwinkel.Domain.Shopping.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.Domain;

public static class FietsenWinkelDomainModule
{
    public static IServiceCollection RegisterFietsenwinkelDomainModule(this IServiceCollection services)
    {
        services.AddScoped<IDetermineBestFietsForKlantService, DetermineBestFietsForKlantService>();

        return services;
    }
}