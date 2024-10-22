using Fietsenwinkel.Api.Voorraden.Models.Out;
using Fietsenwinkel.Domain.Voorraden.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Fietsenwinkel.Api.Voorraden.Mappers;
internal static class VoorraadListMapper
{
    public static VoorraadListOutputModel Map(Voorraad voorraad) =>
        new VoorraadListOutputModel(Map(voorraad.VoorraadEntries));

    internal static VoorraadListEntryOutputModel[] Map(IEnumerable<VoorraadEntry> voorraadEntries) =>
        voorraadEntries.Select(Map).ToArray();

    internal static VoorraadListEntryOutputModel Map(VoorraadEntry voorraadEntry) =>
        new VoorraadListEntryOutputModel(voorraadEntry.FietsType.Value, voorraadEntry.Aantal);
}
