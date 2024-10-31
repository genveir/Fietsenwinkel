using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Shopping.Plugins;

public interface IFietsReserver
{
    Task<ErrorResult<ErrorCodeList>> ReserveFietsForUser(Fiets fiets, Klant klant);
}