using System;

namespace Fietsenwinkel.Shared.Results;

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