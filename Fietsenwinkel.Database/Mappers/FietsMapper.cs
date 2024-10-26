using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using System.Collections.Generic;

namespace Fietsenwinkel.Database.Mappers;

internal class FietsMapper
{
    internal static Result<Fiets, ErrorCodeSet> Map(FietsModel fiets)
    {
        ErrorCodeSet errors = [];

        AantalWielen aantalWielen = AantalWielen.Default();
        AantalWielen.Create(fiets.AantalWielen).Switch(
            onSuccess: a => aantalWielen = a,
            onFailure: errors.AddRange);

        FietsType fietsType = FietsType.Default();
        FietsType.Create(fiets.FietsType.TypeName).Switch(
            onSuccess: ft => fietsType = ft,
            onFailure: errors.AddRange);

        FrameMaat frameMaat = FrameMaat.Default();
        FrameMaat.Create(fiets.FrameMaat).Switch(
            onSuccess: fm => frameMaat = fm,
            onFailure: errors.AddRange);

        Money price = Money.Default();
        Money.Create(fiets.Price).Switch(
            onSuccess: p => price = p,
            onFailure: errors.AddRange);

        var mappedFiets = new Fiets(fietsType, aantalWielen, frameMaat, price);

        if (errors.Count > 0)
        {
            return Result<Fiets, ErrorCodeSet>.Fail(errors);
        }

        return Result<Fiets, ErrorCodeSet>.Succeed(mappedFiets);
    }
}