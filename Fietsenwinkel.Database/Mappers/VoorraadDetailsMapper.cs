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
    public static Result<VoorraadDetails, ErrorCodeList> Map(FiliaalId filiaalId, FietsModel[] fietsen)
    {
        ErrorCodeList errors = [];

        Fiets[] mappedFietsen = [];
        MapFietsen(fietsen).Switch(
            onSuccess: f => mappedFietsen = f,
            onFailure: errors.AddRange);

        if (errors.Count > 0)
        {
            return Result<VoorraadDetails, ErrorCodeList>.Fail(errors);
        }

        var voorraadDetails = new VoorraadDetails(filiaalId, mappedFietsen);

        return Result<VoorraadDetails, ErrorCodeList>.Succeed(voorraadDetails);
    }

    private static Result<Fiets[], ErrorCodeList> MapFietsen(FietsModel[] fietsen)
    {
        ErrorCodeList errors = [];

        List<Fiets> mappedFietsen = [];

        foreach (var fiets in fietsen)
        {
            FietsMapper.Map(fiets).Switch(
                onSuccess: mappedFietsen.Add,
                onFailure: errors.AddRange);
        }

        if (errors.Count > 0)
        {
            return Result<Fiets[], ErrorCodeList>.Fail(errors);
        }

        return Result<Fiets[], ErrorCodeList>.Succeed([.. mappedFietsen]);
    }
}