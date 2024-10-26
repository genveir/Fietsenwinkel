using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Shopping.Plugins;

public interface IFietsInBudgetResolver
{
    Task<Result<Fiets, ErrorCodeSet>> GetFiets(FiliaalId filiaal, Money budget, FrameMaat min, FrameMaat max, FietsType type);

    Task<Result<Fiets, ErrorCodeSet>> GetFiets(FiliaalId filiaal, Money budget, FrameMaat min, FrameMaat max);
}