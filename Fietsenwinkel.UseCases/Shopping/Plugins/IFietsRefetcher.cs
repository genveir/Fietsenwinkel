using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Shopping.Plugins;

public interface IFietsRefetcher
{
    Task<Result<Fiets, ErrorCodeList>> RefetchFiets(Fiets fiets);
}