namespace MapsApiClient;

[Serializable]
public class KanIkNietVindenException : Exception
{
    public KanIkNietVindenException()
    {
    }

    public KanIkNietVindenException(string? message) : base(message)
    {
    }

    public KanIkNietVindenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}