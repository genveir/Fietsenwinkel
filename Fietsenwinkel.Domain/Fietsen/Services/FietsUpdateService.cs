using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Fietsen.Plugins;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Filialen.Plugins;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Fietsen.Services;

public record FietsUpdateModel(FiliaalId Filiaal, FietsId FietsId, FietsType Type, AantalWielen AantalWielen, FrameMaat FrameMaat, Money Price);

public interface IFietsUpdateService
{
    Task<Result<Fiets, ErrorCodeList>> UpdateFiets(FietsUpdateModel fietsUpdateModel);
}

internal class FietsUpdateService : IFietsUpdateService
{
    private readonly IFietsAccessor fietsAccessor;
    private readonly IFietsMover fietsMover;
    private readonly IFiliaalExistenceChecker filiaalExistenceChecker;
    private readonly IFietsUpdater fietsUpdater;

    public FietsUpdateService(IFietsAccessor fietsAccessor, IFietsMover fietsMover, IFiliaalExistenceChecker filiaalExistenceChecker, IFietsUpdater fietsUpdater)
    {
        this.fietsAccessor = fietsAccessor;
        this.fietsMover = fietsMover;
        this.filiaalExistenceChecker = filiaalExistenceChecker;
        this.fietsUpdater = fietsUpdater;
    }

    // Volgens de PO is het verplaatsen van een fiets naar een ander filiaal ook gewoon altijd onderdeel
    // van een normale aanpassing. Daarmee is het combineren van de verplaatsing en aanpassen van de
    // feitelijke fiets een domein-concern. Als het toevallig gecombineerd werd in de api-call maar je
    // kon het ook los doen via andere calls of een file-import oid dan was het een usecase-concern.
    public async Task<Result<Fiets, ErrorCodeList>> UpdateFiets(FietsUpdateModel fietsUpdateModel)
    {
        return await Result.Combine(
            await fietsAccessor.FietsExists(fietsUpdateModel.FietsId),
            await filiaalExistenceChecker.Exists(fietsUpdateModel.Filiaal))
            .Map(
                onSuccess: MoveFiets,
                onFailure: Result<Fiets, ErrorCodeList>.FailAsTask);

        async Task<Result<Fiets, ErrorCodeList>> MoveFiets()
        {
            var moveResult = await fietsMover.MoveFietsToVoorraadOfFiliaal(fietsUpdateModel.FietsId, fietsUpdateModel.Filiaal);

            return await moveResult
                .Map(
                    onSuccess: UpdateFiets,
                    onFailure: Result<Fiets, ErrorCodeList>.FailAsTask);
        }

        async Task<Result<Fiets, ErrorCodeList>> UpdateFiets() =>
            await fietsUpdater.Update(new(
                Id: fietsUpdateModel.FietsId,
                FietsType: fietsUpdateModel.Type,
                AantalWielen: fietsUpdateModel.AantalWielen,
                FrameMaat: fietsUpdateModel.FrameMaat,
                Price: fietsUpdateModel.Price
                ));
    }
}
