using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Fietsen.Plugins;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Admin.Voorraden;

public interface IGetFietsUseCase
{
    Task<Result<Fiets, ErrorCodeList>> GetFiets(FiliaalId filiaalId, FietsId fietsId);
}

internal class GetFietsUseCase : IGetFietsUseCase
{
    private readonly IFietsAccessor fietsAccessor;

    public GetFietsUseCase(IFietsAccessor fietsAccessor)
    {
        this.fietsAccessor = fietsAccessor;
    }

    public async Task<Result<Fiets, ErrorCodeList>> GetFiets(FiliaalId filiaalId, FietsId fietsId) =>
        await fietsAccessor.GetFietsAtFiliaal(fietsId, filiaalId);
}
