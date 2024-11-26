using Fietsenwinkel.Database.Models;
using Fietsenwinkel.UseCases.Voorbeeld.Plugins;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Voorbeeld;

internal class DatabaseResetter : IDatabaseResetter
{
    private static readonly Random random = new();

    public async Task ResetDatabase()
    {
        using var db = new FietsenwinkelContext();

        _ = db.Database.SqlQuery<int>($"delete from Fietsen").ToArray();
        _ = db.Database.SqlQuery<int>($"delete from Voorraden").ToArray();
        _ = db.Database.SqlQuery<int>($"delete from Filialen").ToArray();
        _ = db.Database.SqlQuery<int>($"delete from FietsTypes").ToArray();

        FietsTypeModel[] fietsTypes = [
                new() { TypeName = "Giant Contend" }, // 1000
                new() { TypeName = "Giant TCR" }, // 2500
                new() { TypeName = "Giant Escape" }, // 700
                new() { TypeName = "Trek Domane" }, // 900
                new() { TypeName = "Trek Emonda" }, // 2100
                new() { TypeName = "Trek Marlin" } // 500
            ];

        FiliaalModel[] filialen = [
                new() { Name = "Geerten's Fietsenwinkel" },
                new() { Name = "Ben's Fietsenwinkel" },
                new() { Name = "Gert-Jan's Fietsenwinkel" },
                new() { Name = "Earde's Fietsenwinkel" }
            ];

        VoorraadModel[] voorraden = [
                new() { Filiaal = filialen[0]},
                new() { Filiaal = filialen[1]},
                new() { Filiaal = filialen[2]},
                new() { Filiaal = filialen[3]},
                new() { Filiaal = filialen[0]}
            ];

        for (int n = 0; n < 4; n++)
        {
            filialen[n].Voorraden.Add(voorraden[n]);
        }
        filialen[0].Voorraden.Add(voorraden[4]);

        db.Filialen.AddRange(filialen);
        db.FietsTypes.AddRange(fietsTypes);

        await db.SaveChangesAsync();

        var dbVoorraden = await db.Voorraden.ToArrayAsync();
        var dbFietsTypes = await db.FietsTypes.ToArrayAsync();

        foreach (var voorraad in dbVoorraden)
        {
            FillVoorraad(voorraad, dbFietsTypes);
        }

        await db.SaveChangesAsync();
    }

    private static void FillVoorraad(VoorraadModel voorraad, FietsTypeModel[] fietsTypes)
    {
        (int weight, FietsTypeModel fietsType)[] weighted = [
            (2000, fietsTypes[0]),
            (500, fietsTypes[1]),
            (2300, fietsTypes[2]),
            (2100, fietsTypes[3]),
            (900, fietsTypes[4]),
            (2500, fietsTypes[5])];

        for (int n = 0; n < 100; n++)
        {
            voorraad.Fietsen.Add(GenerateFiets(weighted));
        }
    }

    private static FietsModel GenerateFiets((int weight, FietsTypeModel fietsType)[] weightedTypes)
    {
        var fietsType = GetRandomFietsType(weightedTypes);
        var maat = random.Next(48, 66);

        var price = 2500 - weightedTypes.Single(wt => wt.fietsType == fietsType).weight;
        price += random.Next(500, 1500);

        return new FietsModel() { FietsType = fietsType, FrameMaat = maat, AantalWielen = 2, Price = price };
    }

    private static FietsTypeModel GetRandomFietsType((int weight, FietsTypeModel fietsType)[] weightedTypes)
    {
        var max = weightedTypes.Sum(w => w.weight);

        var roll = random.Next(max);

        int weight = 0;
        for (int n = 0; n < weightedTypes.Length; n++)
        {
            weight += weightedTypes[n].weight;

            if (roll <= weight) return weightedTypes[n].fietsType;
        }

        throw new InvalidOperationException("dit zou niet moeten kunnen gebeuren");
    }
}