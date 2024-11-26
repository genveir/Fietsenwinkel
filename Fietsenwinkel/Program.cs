using Fietsenwinkel.Api;
using Fietsenwinkel.Database;
using Fietsenwinkel.Domain;
using Fietsenwinkel.MapsApiAdapter;
using Fietsenwinkel.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.RegisterFietsenwinkelApiModule();
        services.RegisterFietsenwinkelDatabaseModule();
        services.RegisterFietsenwinkelUseCaseModule();
        services.RegisterFietsenwinkelDomainModule();
        services.RegisterFietsenwinkelMapsApiAdapter();

        services.AddControllers();
    }
}