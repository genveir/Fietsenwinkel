using Microsoft.Extensions.DependencyInjection;

namespace MapsApiClient;

public static class MapsApiClientModule
{
    public static IServiceCollection AddMapsApi(this IServiceCollection services)
    {
        services.AddSingleton<IMapsApiClient, Client>();

        return services;
    }
}