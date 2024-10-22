using System;

namespace Fietsenwinkel.Shared;

public abstract class Result<TValueType>
{
    public static Result<TValueType> Succeed(TValueType value) => new SuccessResult<TValueType>(value);

    public static Result<TValueType> Fail() => new FailureResult<TValueType>();

    public TReturnType Swich<TReturnType>(
        Func<TValueType, TReturnType> onSuccess,
        Func<TReturnType> onFailure)
    {
        return this switch
        {
            SuccessResult<TValueType> success => onSuccess(success.Value),
            FailureResult<TValueType> failure => onFailure(),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}

public class SuccessResult<TValueType> : Result<TValueType>
{
    public TValueType Value { get; }

    public SuccessResult(TValueType value)
    {
        Value = value;
    }
}

public class FailureResult<TValueType> : Result<TValueType> { }

public abstract class Result<TValueType, TErrorType>
{
    public static Result<TValueType, TErrorType> Succeed(TValueType value) =>
        new SuccessResult<TValueType, TErrorType>(value);

    public static Result<TValueType, TErrorType> Fail(TErrorType error) =>
        new FailureResult<TValueType, TErrorType>(error);

    public TReturnType Switch<TReturnType>(
        Func<TValueType, TReturnType> onSuccess,
        Func<TErrorType, TReturnType> onFailure)
    {
        return this switch
        {
            SuccessResult<TValueType, TErrorType> success => onSuccess(success.Value),
            FailureResult<TValueType, TErrorType> failure => onFailure(failure.Error),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}

public class SuccessResult<TValueType, TErrorType> : Result<TValueType, TErrorType>
{
    public TValueType Value { get; }

    public SuccessResult(TValueType value)
    {
        Value = value;
    }
}

public class FailureResult<TValueType, TErrorType> : Result<TValueType, TErrorType>
{
    public TErrorType Error { get; }

    public FailureResult(TErrorType error)
    {
        Error = error;
    }
}
