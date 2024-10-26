using Fietsenwinkel.Api.Voorraden.Models.Out;
using Fietsenwinkel.Domain.Voorraden.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Fietsenwinkel.Api.Voorraden.Mappers;

internal static class VoorraadListMapper
{
    public static VoorraadListOutputModel Map(VoorraadList voorraadList) =>
        new(Map(voorraadList.VoorraadEntries));

    internal static VoorraadListEntryOutputModel[] Map(IEnumerable<VoorraadListEntry> voorraadEntries) =>
        voorraadEntries.Select(Map).ToArray();

    internal static VoorraadListEntryOutputModel Map(VoorraadListEntry voorraadEntry) =>
        new(voorraadEntry.FietsType.Value, voorraadEntry.Aantal);
}