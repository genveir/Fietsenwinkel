using Fietsenwinkel.Api;
using Fietsenwinkel.Database;
using Fietsenwinkel.Domain;
using Fietsenwinkel.MapsApiAdapter;
using Fietsenwinkel.UseCases;
using Fietsenwinkel.UseCases.Admin;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fietsenwinkel;

public class Program
{
    public static void Main(string[] args)
    {
        HijackDeRunnerEvenOmVoorbeeldjesTeDraaien();

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
        services.RegisterFietsenwinkelUseCaseAdminModule();
        services.RegisterFietsenwinkelDomainModule();
        services.RegisterFietsenwinkelMapsApiAdapter();

        services.AddControllers();
    }

    private static void HijackDeRunnerEvenOmVoorbeeldjesTeDraaien()
    {
        var slechtTestje = Shared.SlechtResultVoorbeeldje.AppendWords("dit ", "is ", "een ", "test");
        var testje = Shared.ResultVoorbeeldje.AppendWords("dit ", "is ", "een ", "test");

        var slechteError = Shared.SlechtResultVoorbeeldje.AppendWords("dit ", "is ", "een ", "paard");
        var testjeError = Shared.ResultVoorbeeldje.AppendWords("dit ", "is ", "een ", "paard");

        Console.WriteLine("Resultaten van de voorbeeldjes:");
        Console.WriteLine("Slecht resultaat: " + slechtTestje);
        Console.WriteLine("Goed resultaat: " + testje);

        Console.WriteLine("Slecht resultaat met error: " + slechteError);
        Console.WriteLine("Goed resultaat met error: " + testjeError);
        Console.WriteLine();
    }
}