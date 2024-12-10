using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Admin.Voorraden.Plugins;
public interface IFietsDeleter
{
    Task<ErrorResult<ErrorCodeList>> DeleteFiets(FietsId id);
}
