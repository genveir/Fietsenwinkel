using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System.Collections.Generic;

namespace Fietsenwinkel.Domain;

public interface IDomainValueType<TValueType, TImplementationType>
    where TImplementationType : IDomainValueType<TValueType, TImplementationType>
{
    public TValueType Value { get; }

    public abstract static bool IsValidDomainTypeFor(TValueType value);

    public abstract static Result<TImplementationType, ErrorCodeSet> Create(TValueType value);

    public abstract static Result<TImplementationType[], ErrorCodeSet> Create(IEnumerable<TValueType> values);

    public abstract static Result<TImplementationType, ErrorCodeSet> Parse(string value);

    public abstract static Result<TImplementationType[], ErrorCodeSet> Parse(IEnumerable<string> values);
}