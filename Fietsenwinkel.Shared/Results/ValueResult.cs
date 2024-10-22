using System;

namespace Fietsenwinkel.Shared.Results;

public abstract class ValueResult<TValueType>
{
    public static ValueResult<TValueType> Succeed(TValueType value) => new ValueSuccessResult<TValueType>(value);

    public static ValueResult<TValueType> Fail() => new ValueFailureResult<TValueType>();

    public TReturnType Swich<TReturnType>(
        Func<TValueType, TReturnType> onSuccess,
        Func<TReturnType> onFailure)
    {
        return this switch
        {
            ValueSuccessResult<TValueType> success => onSuccess(success.Value),
            ValueFailureResult<TValueType> failure => onFailure(),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}

public class ValueSuccessResult<TValueType> : ValueResult<TValueType>
{
    public TValueType Value { get; }

    public ValueSuccessResult(TValueType value)
    {
        Value = value;
    }
}

public class ValueFailureResult<TValueType> : ValueResult<TValueType> { }