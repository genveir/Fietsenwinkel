using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Fietsen.Services;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden.Management;

public record FietsUpdateModel(FiliaalId Filiaal, FietsId FietsId, FietsType Type, AantalWielen AantalWielen, FrameMaat FrameMaat, Money Price);

public interface IUpdateFietsUseCase
{
    public Task<Result<Fiets, ErrorCodeList>> Update(FietsUpdateModel fietsUpdateModel);
}

internal class UpdateFietsUseCase : IUpdateFietsUseCase
{
    private readonly IFietsUpdateService fietsUpdater;

    public UpdateFietsUseCase(IFietsUpdateService fietsUpdater)
    {
        this.fietsUpdater = fietsUpdater;
    }

    public async Task<Result<Fiets, ErrorCodeList>> Update(FietsUpdateModel fietsUpdateModel) =>
        await fietsUpdater.UpdateFiets(new(
                Filiaal: fietsUpdateModel.Filiaal,
                FietsId: fietsUpdateModel.FietsId,
                Type: fietsUpdateModel.Type,
                AantalWielen: fietsUpdateModel.AantalWielen,
                FrameMaat: fietsUpdateModel.FrameMaat,
                Price: fietsUpdateModel.Price
            ));
}
