using Fietsenwinkel.Domain.Fietsen.Entities;
using System;

namespace Fietsenwinkel.Domain.Voorraden.Entities;
public class VoorraadEntry
{
    public FietsType FietsType { get; set; }
    public int Aantal { get; set; }

    public VoorraadEntry(FietsType fietsType, int aantal)
    {
        FietsType = fietsType;

        if (aantal < 0)
        {
            throw new ArgumentException("Aantal fietsen in voorraad kan niet negatief zijn");
        }

        Aantal = aantal;
    }
}
