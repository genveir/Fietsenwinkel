﻿using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using System.Collections.Generic;

namespace Fietsenwinkel.Database.Mappers;

internal static class VoorraadDetailsMapper
{
    public static Result<VoorraadDetails, ErrorCodeList> Map(FiliaalId filiaalId, FietsModel[] fietsen) =>
        MapFietsen(fietsen).Map(
            onSuccess: mappedFietsen => Result<VoorraadDetails, ErrorCodeList>.Succeed(new(filiaalId, mappedFietsen)),
            onFailure: Result<VoorraadDetails, ErrorCodeList>.Fail);

    private static Result<Fiets[], ErrorCodeList> MapFietsen(FietsModel[] fietsen)
    {
        ErrorCodeList errors = [];
        List<Fiets> mappedFietsen = [];

        foreach (var fiets in fietsen)
        {
            FietsMapper.Map(fiets).Act(
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