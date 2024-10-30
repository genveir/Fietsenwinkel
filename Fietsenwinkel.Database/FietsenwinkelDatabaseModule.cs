using Fietsenwinkel.Database.Voorbeeld;
using Fietsenwinkel.Database.Voorraden;
using Fietsenwinkel.UseCases.Voorbeeld.Plugins;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.Database;

public static class FietsenwinkelDatabaseModule
{
    public static IServiceCollection RegisterFietsenwinkelDatabaseModule(this IServiceCollection services)
    {
        services.AddScoped<IVoorraadListAccessor, VoorraadRepository>();
        services.AddScoped<IDatabaseResetter, DatabaseResetter>();

        return services;
    }
}