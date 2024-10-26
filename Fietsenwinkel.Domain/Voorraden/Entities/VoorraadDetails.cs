using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;

namespace Fietsenwinkel.Domain.Voorraden.Entities;

public record VoorraadDetails(FiliaalId FiliaalId, Fiets[] Fietsen);