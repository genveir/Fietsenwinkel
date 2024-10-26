namespace Fietsenwinkel.Domain.Shopping.Entities;

// de int en string zouden ook valuetypes moeten zijn, maar de tijd om hier aan te werken is beperkt
public record Klant(int Height, string Location, Money Budget);