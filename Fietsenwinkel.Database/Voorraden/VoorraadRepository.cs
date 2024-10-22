using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Voorraden;
internal class VoorraadRepository : IVoorraadListAccessor
{
    private VoorraadEntry[] DeVoorraad;

    public VoorraadRepository()
    {
        var types = new List<FietsType>()
        {
            new FietsType("Giant Contend"),
            new FietsType("Giant TCR"),
            new FietsType("Giant Escape"),
            new FietsType("Trek Domane"),
            new FietsType("Trek Emonda"),
            new FietsType("Trek Marlin")
        };

        Random random = new();

        DeVoorraad = new VoorraadEntry[6];
        for (int n = 0; n < 6; n++)
        {
            DeVoorraad[n] = new VoorraadEntry(types[n], random.Next(1, 10));
        }
    }

    public Task<Result<Voorraad, ErrorCodeSet>> ListVoorraad(VoorraadListAccessorQuery query)
    {
        return Task.FromResult(Result<Voorraad, ErrorCodeSet>.Succeed(new Voorraad(DeVoorraad)));
    }
}
