using System;

namespace Fietsenwinkel.Domain.Fietsen.Entities;
public class Fiets
{
    public FietsType Type { get; set; }

    public int AantalWielen { get; set; }

    public Fiets(FietsType type, int aantalWielen)
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
}
