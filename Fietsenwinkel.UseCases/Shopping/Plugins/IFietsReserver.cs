using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Shopping.Plugins;

public interface IFietsReserver
{
    Task<ErrorResult<ErrorCodeSet>> ReserveFietsForUser(Fiets fiets, Klant klant);
}