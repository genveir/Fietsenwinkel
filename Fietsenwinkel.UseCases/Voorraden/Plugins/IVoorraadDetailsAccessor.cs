using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden.Plugins;

public record VoorraadDetailsAccessorQuery(FiliaalId FiliaalId);

public interface IVoorraadDetailsAccessor
{
    Task<Result<VoorraadDetails, ErrorCodeList>> GetVoorraadDetails(VoorraadDetailsAccessorQuery query);
}