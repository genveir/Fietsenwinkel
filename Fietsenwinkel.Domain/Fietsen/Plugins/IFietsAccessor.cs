using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Fietsen.Plugins;
public interface IFietsAccessor
{
    Task<ErrorResult<ErrorCodeList>> FietsExists(FietsId id);

    Task<Result<Fiets, ErrorCodeList>> GetFiets(FietsId id);

    Task<ErrorResult<ErrorCodeList>> FietsExistsAtFiliaal(FietsId fietsId, FiliaalId filiaalId);

    Task<Result<Fiets, ErrorCodeList>> GetFietsAtFiliaal(FietsId fietsId, FiliaalId filiaalId);
}