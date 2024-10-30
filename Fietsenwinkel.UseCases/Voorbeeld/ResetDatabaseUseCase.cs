using Fietsenwinkel.UseCases.Voorbeeld.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorbeeld;

public interface IResetDatabaseUseCase
{
    Task Execute();
}

internal class ResetDatabaseUseCase : IResetDatabaseUseCase
{
    private readonly IDatabaseResetter resetter;

    public ResetDatabaseUseCase(IDatabaseResetter resetter)
    {
        this.resetter = resetter;
    }

    public async Task Execute()
    {
        await resetter.ResetDatabase();
    }
}