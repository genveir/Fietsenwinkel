using Fietsenwinkel.UseCases.Voorbeeld;
using Fietsenwinkel.UseCases.Voorraden;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.UseCases;

public static class FietsenwinkelUseCaseModule
{
    public static void RegisterFietsenwinkelUseCaseModule(this IServiceCollection services)
    {
        services.AddScoped<IListVoorraadUseCase, ListVoorraadUseCase>();
        services.AddScoped<IResetDatabaseUseCase, ResetDatabaseUseCase>();
        services.AddScoped<IListFilialenUseCase, ListFilialenUseCase>();
    }
}