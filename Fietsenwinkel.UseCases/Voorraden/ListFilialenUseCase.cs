using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden;

public interface IListFilialenUseCase
{
    Task<Result<FiliaalList, ErrorCodeSet>> ListFilialen();
}

internal class ListFilialenUseCase : IListFilialenUseCase
{
    private readonly IFiliaalListAccessor filiaalListAccessor;

    public ListFilialenUseCase(IFiliaalListAccessor filiaalListAccessor)
    {
        this.filiaalListAccessor = filiaalListAccessor;
    }

    public async Task<Result<FiliaalList, ErrorCodeSet>> ListFilialen()
    {
        return await filiaalListAccessor.ListFilialen();
    }
}