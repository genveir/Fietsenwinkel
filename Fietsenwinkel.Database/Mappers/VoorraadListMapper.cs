using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using System.Linq;

using Result = Fietsenwinkel.Shared.Results.Result<
    Fietsenwinkel.Domain.Voorraden.Entities.VoorraadList,
    Fietsenwinkel.Domain.Errors.ErrorCodeSet>;

namespace Fietsenwinkel.Database.Mappers;

internal static class VoorraadListMapper
{
    // door intense legacy (ik wou liever niet dealen met het definen dat een single voorraad een child van filiaal is,
    // dus nu heeft een filiaal een collectie van voorraden) moet de mapper om kunnen gaan met meerdere voorraden. Dit is
    // een database concern, in het domein kan het namelijk niet.

    public static Result Map(VoorraadModel[] voorraadModels)
    {
        if (voorraadModels.Length == 0)
        {
            return Result.Fail([ErrorCodes.Voorraad_Not_Found]);
        }

        if (!voorraadModels.All(vm => vm.FiliaalId == voorraadModels.First().FiliaalId))
        {
            return Result.Fail([ErrorCodes.Legacy_Voorraden_Have_Different_FiliaalIds]);
        }

        ErrorCodeSet errors = [];

        FiliaalId filiaalId = FiliaalId.Default();
        FiliaalId.Create(voorraadModels.First().FiliaalId)
            .Switch(
                onSuccess: f => filiaalId = f,
                onFailure: errors.AddRange);

        VoorraadListEntry[] entries = [];
        MapEntries(voorraadModels)
            .Switch(
                onSuccess: e => entries = e,
                onFailure: errors.AddRange);

        if (errors.Count > 0)
        {
            return Result.Fail(errors);
        }

        var voorraadList = new VoorraadList(
            filiaalId!,
            entries);

        return Result.Succeed(voorraadList);
    }

    private static Result<VoorraadListEntry[], ErrorCodeSet> MapEntries(VoorraadModel[] voorraadModels)
    {
        var groupedSupply = voorraadModels
            .SelectMany(vm => vm.Fietsen)
            .GroupBy(f => f.FietsType);

        var result = new VoorraadListEntry[groupedSupply.Count()];

        ErrorCodeSet errors = [];

        int index = 0;
        foreach (var group in groupedSupply)
        {
            var fietsTypeModel = group.Key;
            var number = group.Count();

            FietsType fietsType = FietsType.Default();
            FietsType.Create(fietsTypeModel.TypeName).Switch(
                onSuccess: ft => fietsType = ft,
                onFailure: errors.AddRange);

            result[index] = new VoorraadListEntry(fietsType, number);

            index++;
        }

        return errors.Count == 0 ?
            Result<VoorraadListEntry[], ErrorCodeSet>.Succeed(result) :
            Result<VoorraadListEntry[], ErrorCodeSet>.Fail(errors);
    }
}