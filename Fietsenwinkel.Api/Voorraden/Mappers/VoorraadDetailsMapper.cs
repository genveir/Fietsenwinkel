using Fietsenwinkel.Api.Voorraden.Models.Out;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Fietsenwinkel.Api.Voorraden.Mappers;

internal static class VoorraadDetailsMapper
{
    public static VoorraadDetailsOutputModel Map(VoorraadDetails voorraadDetails) =>
        new(voorraadDetails.FiliaalId.Value, Map(voorraadDetails.Fietsen));

    private static VoorraadDetailsFietsOutputModel[] Map(IEnumerable<Fiets> fietsen) =>
        fietsen.Select(Map).ToArray();

    private static VoorraadDetailsFietsOutputModel Map(Fiets fiets) =>
        new(fiets.Type.Value, fiets.AantalWielen.Value, fiets.FrameMaat.Value);
}