﻿using Fietsenwinkel.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database;

public static class DbReset
{
    private static readonly Random random = new();

    public static async Task ResetDatabase()
    {
        using var db = new FietsenwinkelContext();

        _ = db.Database.SqlQuery<int>($"delete from Voorraden").ToArray();
        _ = db.Database.SqlQuery<int>($"delete from Fietsen").ToArray();
        _ = db.Database.SqlQuery<int>($"delete from FietsTypes").ToArray();
        _ = db.Database.SqlQuery<int>($"delete from Filialen").ToArray();

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
                new() { Name = "Ties' Fietsenwinkel" }
            ];

        VoorraadModel[] voorraden = [
                new() { Filiaal = filialen[0]},
                new() { Filiaal = filialen[1]},
                new() { Filiaal = filialen[2]},
                new() { Filiaal = filialen[3]}
            ];

        foreach (var voorraad in voorraden)
        {
            FillVoorraad(voorraad, fietsTypes);
        }

        db.Filialen.AddRange(filialen);

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

        return new FietsModel() { FietsType = fietsType, FrameMaat = maat };
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