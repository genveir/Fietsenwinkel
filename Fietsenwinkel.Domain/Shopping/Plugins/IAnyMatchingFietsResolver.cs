using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Shopping.Plugins;

public interface IAnyMatchingFietsResolver
{
    Task<Result<Fiets, ErrorCodeList>> GetFiets(FiliaalId filiaal, FrameMaat min, FrameMaat max);
}