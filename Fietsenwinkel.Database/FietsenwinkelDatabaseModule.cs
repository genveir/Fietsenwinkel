using Fietsenwinkel.Database.Fietsen;
using Fietsenwinkel.Database.Filialen;
using Fietsenwinkel.Database.Voorbeeld;
using Fietsenwinkel.Database.Voorraden;
using Fietsenwinkel.Domain.Shopping.Plugins;
using Fietsenwinkel.UseCases.Filialen;
using Fietsenwinkel.UseCases.Shopping.Plugins;
using Fietsenwinkel.UseCases.Voorbeeld.Plugins;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.Database;

public static class FietsenwinkelDatabaseModule
{
    public static IServiceCollection RegisterFietsenwinkelDatabaseModule(this IServiceCollection services)
    {
        services.AddScoped<IVoorraadListAccessor, VoorraadRepository>();
        services.AddScoped<IFiliaalListAccessor, FilialenRepository>();
        services.AddScoped<IShoppingFiliaalListAccessor, FilialenRepository>();
        services.AddScoped<IFiliaalExistenceChecker, FilialenRepository>();
        services.AddScoped<IVoorraadDetailsAccessor, FietsenRepository>();
        services.AddScoped<IDatabaseResetter, DatabaseResetter>();
        services.AddScoped<IFietsRefetcher, FietsenRepository>();
        services.AddScoped<IFietsInBudgetResolver, FietsenRepository>();
        services.AddScoped<IAnyMatchingFietsResolver, FietsenRepository>();

        return services;
    }
}