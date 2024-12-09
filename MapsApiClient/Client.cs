#pragma warning disable IDE0130 // Namespace is not the same as the file path
namespace MapsApiClient;

// dit is een Nuget package, je kan de interface niet verplaatsen
public interface IMapsApiClient
{
    Task<MapCoordinate> ResolvePosition(string thingToLocate);
}

internal class Client : IMapsApiClient
{
    public Task<MapCoordinate> ResolvePosition(string thingToLocate)
    {
        if (thingToLocate.StartsWith("Onvindbaar"))
            throw new KanIkNietVindenException("oh jee niet te vinden");

        return Task.FromResult(new MapCoordinate(GetStableHashCode(thingToLocate)));
    }

    private static int GetStableHashCode(string str)
    {
        unchecked
        {
            int hash1 = 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1 || str[i + 1] == '\0')
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
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