using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using System;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Voorraden;

internal class VoorraadRepository : IVoorraadListAccessor
{
    private static Result<FietsType[], ErrorCodeSet> GetFietsTypes()
    {
        return FietsType.Create([
            "Giant Contend",
            "Giant TCR",
            "Giant Escape",
            "Trek Domane",
            "Trek Emonda",
            "Trek Marlin"
        ]);
    }

    public Task<Result<Voorraad, ErrorCodeSet>> ListVoorraad(VoorraadListAccessorQuery query)
    {
        return GetFietsTypes()
            .Switch(
                onSuccess: ListVoorraad,
                onFailure: errors => Task.FromResult(Result<Voorraad, ErrorCodeSet>.Fail(errors)));
    }

    private Task<Result<Voorraad, ErrorCodeSet>> ListVoorraad(FietsType[] types)
    {
        Random random = new();

        var deVoorraad = new VoorraadEntry[6];
        for (int n = 0; n < 6; n++)
        {
            deVoorraad[n] = new VoorraadEntry(types[n], random.Next(1, 10));
        }

        return Task.FromResult(Result<Voorraad, ErrorCodeSet>.Succeed(new Voorraad(deVoorraad)));
    }
}