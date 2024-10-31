using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden.Plugins;
public record VoorraadListAccessorQuery(FiliaalId Filiaal, string? NameFilter);

public interface IVoorraadListAccessor
{
    Task<Result<VoorraadList, ErrorCodeList>> ListVoorraad(VoorraadListAccessorQuery query);
}