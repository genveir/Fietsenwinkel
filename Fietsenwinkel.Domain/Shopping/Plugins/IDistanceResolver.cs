using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Shopping.Plugins;

public interface IDistanceResolver
{
    Task<Result<int, ErrorCodeList>> ResolveDistanceBetween(string location, FiliaalName filiaalName);
}