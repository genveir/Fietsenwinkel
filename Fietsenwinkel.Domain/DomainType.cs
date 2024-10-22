namespace Fietsenwinkel.Domain;
public interface IDomainType<TValueType, TImplementationType> where TImplementationType : IDomainType<TValueType, TImplementationType>
{
    public TValueType Value { get; }

    public abstract static bool IsValidDomainTypeFor(TValueType value);

    public abstract static bool TryParse(string value, out TImplementationType? domainType);
}
