using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Fietsen.Plugins;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Management.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden.Management;

public record FietsCreateModel(FiliaalId Filiaal, FietsType Type, AantalWielen AantalWielen, FrameMaat FrameMaat, Money Price);

public interface IAddFietsUseCase
{
    Task<Result<Fiets, ErrorCodeList>> Add(FietsCreateModel fietsCreateModel);
}

internal class AddFietsUseCase : IAddFietsUseCase
{
    private readonly IFietsToVoorraadAdder fietsToVoorraadAdder;
    private readonly IFietsAccessor fietsAccessor;

    public AddFietsUseCase(
        IFietsToVoorraadAdder fietsToVoorraadAdder,
        IFietsAccessor fietsAccessor)
    {
        this.fietsToVoorraadAdder = fietsToVoorraadAdder;
        this.fietsAccessor = fietsAccessor;
    }

    public async Task<Result<Fiets, ErrorCodeList>> Add(FietsCreateModel fietsCreateModel)
    {
        var saveResult = await fietsToVoorraadAdder.AddFietsToVoorraad(fietsCreateModel);

        return await saveResult.Map(
            onSuccess: fietsAccessor.GetFiets,
            onFailure: Result<Fiets, ErrorCodeList>.FailAsTask);
    }
}
