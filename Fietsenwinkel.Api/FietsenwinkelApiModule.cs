using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Fietsenwinkel.Api;

public static class FietsenwinkelApiModule
{
    public static void RegisterFietsenwinkelApiModule(this IServiceCollection services)
    {
        Debug.WriteLine($"Eigenlijk doe ik nu niks met {services} maar ik wil graag een mooi lijstje van geregistreerde modules");
    }
}