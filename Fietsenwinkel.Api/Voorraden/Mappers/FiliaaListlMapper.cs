using Fietsenwinkel.Api.Voorraden.Models.Out;
using Fietsenwinkel.Domain.Filialen.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Fietsenwinkel.Api.Voorraden.Mappers;

internal static class FiliaaListlMapper
{
    public static FiliaalListOutputModel Map(FiliaalList filiaalList) =>
        new(Map(filiaalList.FiliaalListEntries));

    private static FiliaalListEntryOutputModel[] Map(IEnumerable<FiliaalListEntry> entries) =>
        entries.Select(Map).ToArray();

    private static FiliaalListEntryOutputModel Map(FiliaalListEntry entry) =>
        new(entry.Id.Value, entry.Name.Value);
}