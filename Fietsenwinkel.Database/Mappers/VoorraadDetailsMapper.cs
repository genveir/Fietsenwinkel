using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using System.Collections.Generic;

namespace Fietsenwinkel.Database.Mappers;

internal static class VoorraadDetailsMapper
{
    public static Result<VoorraadDetails, ErrorCodeSet> Map(FiliaalId filiaalId, FietsModel[] fietsen)
    {
        ErrorCodeSet errors = [];

        Fiets[] mappedFietsen = [];
        MapFietsen(fietsen).Switch(
            onSuccess: f => mappedFietsen = f,
            onFailure: errors.AddRange);

        if (errors.Count > 0)
        {
            return Result<VoorraadDetails, ErrorCodeSet>.Fail(errors);
        }

        var voorraadDetails = new VoorraadDetails(filiaalId, mappedFietsen);

        return Result<VoorraadDetails, ErrorCodeSet>.Succeed(voorraadDetails);
    }

    private static Result<Fiets[], ErrorCodeSet> MapFietsen(FietsModel[] fietsen)
    {
        ErrorCodeSet errors = [];

        List<Fiets> mappedFietsen = [];

        foreach (var fiets in fietsen)
        {
            MapFiets(fiets).Switch(
                onSuccess: mappedFietsen.Add,
                onFailure: errors.AddRange);
        }

        if (errors.Count > 0)
        {
            return Result<Fiets[], ErrorCodeSet>.Fail(errors);
        }

        return Result<Fiets[], ErrorCodeSet>.Succeed([.. mappedFietsen]);
    }

    private static Result<Fiets, ErrorCodeSet> MapFiets(FietsModel fiets)
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

        var mappedFiets = new Fiets(fietsType, aantalWielen, frameMaat);

        if (errors.Count > 0)
        {
            return Result<Fiets, ErrorCodeSet>.Fail(errors);
        }

        return Result<Fiets, ErrorCodeSet>.Succeed(mappedFiets);
    }
}