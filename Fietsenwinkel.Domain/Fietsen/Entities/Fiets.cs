﻿using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public class Fiets
{
    public FietsType Type { get; set; }

    public int AantalWielen { get; set; }

    private Fiets(FietsType type, int aantalWielen)
    {
        Type = type;

        if (aantalWielen < 1)
        {
            throw new ArgumentException("Een fiets moet wielen hebben");
        }
        if (aantalWielen > 3)
        {
            throw new ArgumentException("Driewielers fine, maar met vier wielen is het geen fiets meer");
        }

        AantalWielen = aantalWielen;
    }

    public static Result<Fiets, ErrorCodeSet> Create(FietsType type, int aantalWielen)
    {
        return aantalWielen switch
        {
            < 1 => Result<Fiets, ErrorCodeSet>.Fail([ErrorCodes.Fiets_Has_No_Wheels]),
            > 3 => Result<Fiets, ErrorCodeSet>.Fail([ErrorCodes.Fiets_Has_Too_Many_Wheels]),
            _ => Result<Fiets, ErrorCodeSet>.Succeed(new Fiets(type, aantalWielen))
        };
    }
}