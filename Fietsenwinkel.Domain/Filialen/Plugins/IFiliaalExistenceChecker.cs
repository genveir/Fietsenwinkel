using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Filialen.Plugins;

public interface IFiliaalExistenceChecker
{
    Task<ErrorResult<ErrorCodeList>> Exists(FiliaalId filiaalId);
}