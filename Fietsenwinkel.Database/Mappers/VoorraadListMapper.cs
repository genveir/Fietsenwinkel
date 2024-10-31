using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using System.Linq;

namespace Fietsenwinkel.Database.Mappers;

internal static class VoorraadListMapper
{
    // door intense legacy (ik wou liever niet dealen met het definen dat een single voorraad een child van filiaal is,
    // dus nu heeft een filiaal een collectie van voorraden) moet de mapper om kunnen gaan met meerdere voorraden. Dit is
    // een database concern, in het domein kan het namelijk niet.

    public static Result<VoorraadList, ErrorCodeList> Map(VoorraadModel[] voorraadModels)
    {
        if (voorraadModels.Length == 0)
        {
            return Result<VoorraadList, ErrorCodeList>.Fail([ErrorCodes.Voorraad_Not_Found]);
        }

        if (!voorraadModels.All(vm => vm.FiliaalId == voorraadModels.First().FiliaalId))
        {
            return Result<VoorraadList, ErrorCodeList>.Fail([ErrorCodes.Legacy_Voorraden_Have_Different_FiliaalIds]);
        }

        return Result.Combine(
            FiliaalId.Create(voorraadModels.First().FiliaalId),
            MapEntries(voorraadModels)).Switch(
                onSuccess: (filiaalId, entries) => Result<VoorraadList, ErrorCodeList>.Succeed(new(filiaalId, entries)),
                onFailure: Result<VoorraadList, ErrorCodeList>.Fail);
    }

    private static Result<VoorraadListEntry[], ErrorCodeList> MapEntries(VoorraadModel[] voorraadModels)
    {
        var groupedSupply = voorraadModels
            .SelectMany(vm => vm.Fietsen)
            .GroupBy(f => f.FietsType);

        var result = new VoorraadListEntry[groupedSupply.Count()];

        ErrorCodeList errors = [];

        int index = 0;
        foreach (var group in groupedSupply)
        {
            var fietsTypeModel = group.Key;
            var number = group.Count();

            FietsType.Create(fietsTypeModel.TypeName).Switch(
                onSuccess: ft => result[index] = new VoorraadListEntry(ft, number),
                onFailure: errors.AddRange);

            index++;
        }

        return errors.Count == 0 ?
            Result<VoorraadListEntry[], ErrorCodeList>.Succeed(result) :
            Result<VoorraadListEntry[], ErrorCodeList>.Fail(errors);
    }
}