using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Database.Mappers;

internal class FietsMapper
{
    internal static Result<Fiets, ErrorCodeSet> Map(FietsModel fiets) =>
        Result.Combine(
            AantalWielen.Create(fiets.AantalWielen),
            FietsType.Create(fiets.FietsType.TypeName),
            FrameMaat.Create(fiets.FrameMaat),
            Money.Create(fiets.Price)).Switch(
                onSuccess: vt =>
                {
                    var (aantalWielen, fietsType, frameMaat, price) = vt;

                    return Result<Fiets, ErrorCodeSet>
                        .Succeed(new Fiets(fietsType, aantalWielen, frameMaat, price));
                },
                onFailure: Result<Fiets, ErrorCodeSet>.Fail);
}