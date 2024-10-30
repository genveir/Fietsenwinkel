using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain;

public interface IDomainValueType<TValueType, TImplementationType>
    where TImplementationType : IDomainValueType<TValueType, TImplementationType>
{
    public TValueType Value { get; }

    public abstract static Result<TImplementationType, ErrorCodeSet> Create(TValueType value);
}