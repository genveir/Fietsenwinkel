using Fietsenwinkel.Api;
using Fietsenwinkel.Database;
using Fietsenwinkel.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fietsenwinkel;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureServices(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.

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

        services.AddControllers();
    }
}
