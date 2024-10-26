using Fietsenwinkel.Domain.Shopping.Plugins;
using MapsApiClient;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel.MapsApiAdapter;

public static class FietsenwinkelMapsApiModule
{
    public static IServiceCollection RegisterFietsenwinkelMapsApiAdapter(this IServiceCollection services)
    {
        services.AddScoped<IDistanceResolver, MapsApiLocationResolver>();

        services.AddMapsApi();

        return services;
    }
}