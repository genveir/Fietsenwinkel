using Fietsenwinkel.UseCases.Voorbeeld.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Voorbeeld;

internal class DatabaseResetter : IDatabaseResetter
{
    public async Task ResetDatabase() => await DbReset.ResetDatabase();
}