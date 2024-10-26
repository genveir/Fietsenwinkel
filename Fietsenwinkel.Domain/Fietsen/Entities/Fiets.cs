using Fietsenwinkel.Domain.Shopping.Entities;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public record Fiets(FietsType Type, AantalWielen AantalWielen, FrameMaat FrameMaat, Money Price);