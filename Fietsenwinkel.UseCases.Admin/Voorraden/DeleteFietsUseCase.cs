using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Fietsen.Plugins;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Admin.Voorraden.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Admin.Voorraden;

public interface IDeleteFietsUseCase
{
    Task<ErrorResult<ErrorCodeList>> DeleteFiets(FiliaalId filiaalId, FietsId fietsId);
}

public class DeleteFietsUseCase : IDeleteFietsUseCase
{
    private readonly IFietsDeleter fietsDeleter;
    private readonly IFietsAccessor fietsAccessor;

    public DeleteFietsUseCase(IFietsDeleter fietsDeleter, IFietsAccessor fietsAccessor)
    {
        this.fietsDeleter = fietsDeleter;
        this.fietsAccessor = fietsAccessor;
    }

    public async Task<ErrorResult<ErrorCodeList>> DeleteFiets(FiliaalId filiaalId, FietsId fietsId)
    {
        var existsResult = await fietsAccessor.FietsExistsAtFiliaal(fietsId, filiaalId);

        return await existsResult.Map(
            onSuccess: () => fietsDeleter.DeleteFiets(fietsId),
            onFailure: ErrorResult<ErrorCodeList>.FailAsTask);
    }
}
