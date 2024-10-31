using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Collections.Generic;

namespace Fietsenwinkel.Database.Mappers;

internal static class FiliaalListMapper
{
    public static Result<FiliaalList, ErrorCodeList> Map(FiliaalModel[] filiaalModels)
    {
        ErrorCodeList errors = [];
        List<FiliaalListEntry> filiaalListEntries = [];

        foreach (var fm in filiaalModels)
        {
            Result.Combine(
                FiliaalId.Create(fm.Id),
                FiliaalName.Create(fm.Name)).Switch(
                    onSuccess: vt =>
                    {
                        var (id, name) = vt;
                        filiaalListEntries.Add(new(id, name));
                    },
                    onFailure: errors.AddRange);
        }

        return errors.Count == 0 ?
            Result<FiliaalList, ErrorCodeList>.Succeed(new([.. filiaalListEntries])) :
            Result<FiliaalList, ErrorCodeList>.Fail(errors);
    }
}