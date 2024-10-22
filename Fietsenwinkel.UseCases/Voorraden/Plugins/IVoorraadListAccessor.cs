using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden.Plugins;
public record VoorraadListAccessorQuery(string? NameFilter);

public interface IVoorraadListAccessor
{
    Task<Result<Voorraad, ErrorCodeSet>> ListVoorraad(VoorraadListAccessorQuery query);
}