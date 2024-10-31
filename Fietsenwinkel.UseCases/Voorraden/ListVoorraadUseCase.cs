using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden;

public record ListVoorraadQuery(FiliaalId Filiaal, string? NameFilter);

public interface IListVoorraadUseCase
{
    Task<Result<VoorraadList, ErrorCodeList>> GetVoorraad(ListVoorraadQuery query);
}

internal class ListVoorraadUseCase : IListVoorraadUseCase
{
    private readonly IVoorraadListAccessor voorraadListAccessor;

    public ListVoorraadUseCase(IVoorraadListAccessor voorraadListAccessor)
    {
        this.voorraadListAccessor = voorraadListAccessor;
    }

    public async Task<Result<VoorraadList, ErrorCodeList>> GetVoorraad(ListVoorraadQuery query)
    {
        var accessorQuery = new VoorraadListAccessorQuery(query.Filiaal, query.NameFilter);

        return await voorraadListAccessor.ListVoorraad(accessorQuery);
    }
}