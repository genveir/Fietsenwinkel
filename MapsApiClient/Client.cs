namespace MapsApiClient;

// dit is een Nuget package, je kan de interface niet verplaatsen
public interface IMapsApiClient
{
    Task<MapCoordinate> ResolvePosition(string thingToLocate);
}

public class Client : IMapsApiClient
{
    public Task<MapCoordinate> ResolvePosition(string thingToLocate)
    {
        if (thingToLocate.StartsWith("Onvindbaar"))
            throw new KanIkNietVindenException("oh jee niet te vinden");

        return Task.FromResult(new MapCoordinate(thingToLocate.GetHashCode()));
    }
}

public class MapCoordinate
{
    private int Position { get; }

    public MapCoordinate(int position)
    {
        Position = position;
    }

    public int GetDistanceTo(MapCoordinate other)
    {
        return Math.Abs(Position - other.Position);
    }
}