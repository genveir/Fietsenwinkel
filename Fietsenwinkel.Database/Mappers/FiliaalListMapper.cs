using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using System.Linq;

using Result = Fietsenwinkel.Shared.Results.Result<
    Fietsenwinkel.Domain.Filialen.Entities.FiliaalList,
    Fietsenwinkel.Domain.Errors.ErrorCodeSet>;

namespace Fietsenwinkel.Database.Mappers;

internal static class FiliaalListMapper
{
    public static Result Map(FiliaalModel[] filiaalModels)
    {
        ErrorCodeSet errors = [];

        var entries = filiaalModels
            .Select(fm =>
            {
                FiliaalId id = FiliaalId.Default();
                FiliaalId.Create(fm.Id).Switch(
                    onSuccess: result => id = result,
                    onFailure: _ => errors.Add(ErrorCodes.FiliaalId_Invalid_Format));

                FiliaalName name = FiliaalName.Default();
                FiliaalName.Create(fm.Name).Switch(
                    onSuccess: n => name = n,
                    onFailure: _ => errors.Add(ErrorCodes.FiliaalName_Invalid_Format));

                return new FiliaalListEntry(id, name);
            }).ToArray();

        return errors.Count == 0 ?
            Result.Succeed(new(entries)) :
            Result.Fail(errors);
    }
}