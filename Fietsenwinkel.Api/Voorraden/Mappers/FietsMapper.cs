using Fietsenwinkel.Api.Voorraden.Models.Out;
using Fietsenwinkel.Domain.Fietsen.Entities;

namespace Fietsenwinkel.Api.Voorraden.Mappers;

internal static class FietsMapper
{
    public static FietsOutputModel Map(Fiets fiets) =>
        new(fiets.Id.Value, fiets.Type.Value, fiets.AantalWielen.Value, fiets.FrameMaat.Value, fiets.Price.Value);
}
