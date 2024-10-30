using System;

namespace Fietsenwinkel.Shared.Results;

public abstract class ValueResult<TValueType>
{
    public static ValueResult<TValueType> Succeed(TValueType value) => new ValueSuccessResult<TValueType>(value);

    public static ValueResult<TValueType> Fail() => new ValueFailureResult<TValueType>();

    public void Switch(
        Action<TValueType> onSuccess,
        Action onFailure)
    {
        switch (this)
        {
            case ValueSuccessResult<TValueType> success:
                onSuccess(success.Value);
                break;
            case ValueFailureResult<TValueType>:
                onFailure();
                break;
        }
    }

    public TReturnType Switch<TReturnType>(
        Func<TValueType, TReturnType> onSuccess,
        Func<TReturnType> onFailure)
    {
        return this switch
        {
            ValueSuccessResult<TValueType> success => onSuccess(success.Value),
            ValueFailureResult<TValueType> => onFailure(),
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