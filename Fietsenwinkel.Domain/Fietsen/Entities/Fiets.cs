using Fietsenwinkel.Domain.Shopping.Entities;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public record Fiets(FietsId Id, FietsType Type, AantalWielen AantalWielen, FrameMaat FrameMaat, Money Price);