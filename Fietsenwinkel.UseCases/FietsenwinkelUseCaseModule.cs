using Fietsenwinkel.UseCases.Voorraden;
using Fietsenwinkel.UseCases.Voorraden.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.UseCases;
public static class FietsenwinkelUseCaseModule
{
    public static void RegisterFietsenwinkelUseCaseModule(this IServiceCollection services)
    {
        services.AddScoped<IListVoorraadUseCase, ListVoorraadUseCase>();
    }
}
