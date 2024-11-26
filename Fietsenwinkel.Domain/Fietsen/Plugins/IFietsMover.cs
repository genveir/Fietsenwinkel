using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Fietsen.Plugins;
public interface IFietsMover
{
    Task<ErrorResult<ErrorCodeList>> MoveFietsToVoorraadOfFiliaal(FietsId fietsId, FiliaalId filiaalId);
}
