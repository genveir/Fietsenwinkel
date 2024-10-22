using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Abstractions;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden;

public record ListVoorraadQuery(string? NameFilter);

internal class ListVoorraadUseCase : IListVoorraadUseCase
{
    private readonly IVoorraadListAccessor voorraadListAccessor;

    public ListVoorraadUseCase(IVoorraadListAccessor voorraadListAccessor)
    {
        this.voorraadListAccessor = voorraadListAccessor;
    }

    public async Task<Result<Voorraad, ErrorCodeSet>> GetVoorraad(ListVoorraadQuery query)
    {
        var accessorQuery = new VoorraadListAccessorQuery(query.NameFilter);

        return await voorraadListAccessor.ListVoorraad(accessorQuery);
    }
}
