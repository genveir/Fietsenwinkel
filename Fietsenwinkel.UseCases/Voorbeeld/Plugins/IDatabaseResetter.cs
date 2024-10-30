using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorbeeld.Plugins;

public interface IDatabaseResetter
{
    Task ResetDatabase();
}