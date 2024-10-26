using Fietsenwinkel.Application.Reservations;
using Fietsenwinkel.UseCases.Shopping.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.Application;

public static class FietsenwinkelApplicationModule
{
    public static IServiceCollection RegisterFietsenwinkelApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IFietsReserver, FietsReserver>();

        return services;
    }
}