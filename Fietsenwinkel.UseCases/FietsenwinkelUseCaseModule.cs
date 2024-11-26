using Fietsenwinkel.UseCases.Shopping;
using Fietsenwinkel.UseCases.Voorbeeld;
using Fietsenwinkel.UseCases.Voorraden;
using Fietsenwinkel.UseCases.Voorraden.Management;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.UseCases;

public static class FietsenwinkelUseCaseModule
{
    public static void RegisterFietsenwinkelUseCaseModule(this IServiceCollection services)
    {
        services.AddScoped<IListVoorraadUseCase, ListVoorraadUseCase>();
        services.AddScoped<IResetDatabaseUseCase, ResetDatabaseUseCase>();
        services.AddScoped<IListFilialenUseCase, ListFilialenUseCase>();
        services.AddScoped<IGetVoorraadDetailsUseCase, GetVoorraadDetailsUseCase>();
        services.AddScoped<IFietsSearchUseCase, FietsSearchUseCase>();

        services.AddScoped<IAddFietsUseCase, AddFietsUseCase>();
        services.AddScoped<IGetFietsUseCase, GetFietsUseCase>();
        services.AddScoped<IUpdateFietsUseCase, UpdateFietsUseCase>();
        services.AddScoped<IDeleteFietsUseCase, DeleteFietsUseCase>();
    }
}