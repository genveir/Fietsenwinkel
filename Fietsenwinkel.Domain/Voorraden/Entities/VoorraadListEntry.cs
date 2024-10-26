using Fietsenwinkel.Domain.Fietsen.Entities;
using System;

namespace Fietsenwinkel.Domain.Voorraden.Entities;

public class VoorraadListEntry
{
    public FietsType FietsType { get; set; }
    public int Aantal { get; set; }

    public VoorraadListEntry(FietsType fietsType, int aantal)
    {
        FietsType = fietsType;

        if (aantal < 0)
        {
            throw new ArgumentException("Aantal fietsen in voorraad kan niet negatief zijn");
        }

        Aantal = aantal;
    }
}