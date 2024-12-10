using Fietsenwinkel.UseCases.Admin.Voorraden;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.UseCases.Admin;
public static class FietsenwinkelUseCaseAdminModule
{
    public static void RegisterFietsenwinkelUseCaseAdminModule(this IServiceCollection services)
    {
        services.AddScoped<IAddFietsUseCase, AddFietsUseCase>();
        services.AddScoped<IGetFietsUseCase, GetFietsUseCase>();
        services.AddScoped<IUpdateFietsUseCase, UpdateFietsUseCase>();
        services.AddScoped<IDeleteFietsUseCase, DeleteFietsUseCase>();
    }
}
