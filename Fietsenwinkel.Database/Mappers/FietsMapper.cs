using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Database.Mappers;

internal class FietsMapper
{
    internal static Result<Fiets, ErrorCodeList> Map(FietsModel fiets) =>
        FietsId.Create(fiets.Id)
            .Map(
                onSuccess: id => MapFiets(id, fiets),
                onFailure: Result<Fiets, ErrorCodeList>.Fail);

    private static Result<Fiets, ErrorCodeList> MapFiets(FietsId id, FietsModel fiets) =>
        Result.Combine(
            AantalWielen.Create(fiets.AantalWielen),
            FietsType.Create(fiets.FietsType.TypeName),
            FrameMaat.Create(fiets.FrameMaat),
            Money.Create(fiets.Price)).Map(
                onSuccess: (aantalWielen, fietsType, frameMaat, price) => MapFiets(id, aantalWielen, fietsType, frameMaat, price),
                onFailure: Result<Fiets, ErrorCodeList>.Fail);

    private static Result<Fiets, ErrorCodeList> MapFiets(FietsId id, AantalWielen aantalWielen, FietsType fietsType, FrameMaat frameMaat, Money price)
    {
        var fiets = new Fiets(id, fietsType, aantalWielen, frameMaat, price);

        return Result<Fiets, ErrorCodeList>.Succeed(fiets);
    }
}