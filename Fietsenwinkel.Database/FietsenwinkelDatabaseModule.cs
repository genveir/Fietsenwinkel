using Fietsenwinkel.Database.Voorraden;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.Database;
public static class FietsenwinkelDatabaseModule
{
    public static IServiceCollection RegisterFietsenwinkelDatabaseModule(this IServiceCollection services)
    {
        services.AddScoped<IVoorraadListAccessor, VoorraadRepository>();

        return services;
    }
}
