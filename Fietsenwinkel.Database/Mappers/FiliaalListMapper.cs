using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Collections.Generic;

namespace Fietsenwinkel.Database.Mappers;

internal static class FiliaalListMapper
{
    public static Result<FiliaalList, ErrorCodeSet> Map(FiliaalModel[] filiaalModels)
    {
        ErrorCodeSet errors = [];
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
            Result<FiliaalList, ErrorCodeSet>.Succeed(new([.. filiaalListEntries])) :
            Result<FiliaalList, ErrorCodeSet>.Fail(errors);
    }
}