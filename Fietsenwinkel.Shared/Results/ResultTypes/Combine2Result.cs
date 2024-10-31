using System;

namespace Fietsenwinkel.Shared.Results;

public abstract class Result<TValueType1, TValueType2, TErrorType>
{
    public static Result<TValueType1, TValueType2, TErrorType> Succeed(TValueType1 value1, TValueType2 value2) =>
        new SuccessResult<TValueType1, TValueType2, TErrorType>(value1, value2);

    public static Result<TValueType1, TValueType2, TErrorType> Fail(TErrorType error) =>
        new FailureResult<TValueType1, TValueType2, TErrorType>(error);

    public void Switch(
        Action<TValueType1, TValueType2> onSuccess,
        Action<TErrorType> onFailure)
    {
        switch (this)
        {
            case SuccessResult<TValueType1, TValueType2, TErrorType> success:
                onSuccess(success.Value1, success.Value2);
                break;

            case FailureResult<TValueType1, TValueType2, TErrorType> failure:
                onFailure(failure.Error);
                break;
        }
    }

    public TReturnType Switch<TReturnType>(
        Func<TValueType1, TValueType2, TReturnType> onSuccess,
        Func<TErrorType, TReturnType> onFailure)
    {
        return this switch
        {
            SuccessResult<TValueType1, TValueType2, TErrorType> success => onSuccess(success.Value1, success.Value2),
            FailureResult<TValueType1, TValueType2, TErrorType> failure => onFailure(failure.Error),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}

public class SuccessResult<TValueType1, TValueType2, TErrorType> : Result<TValueType1, TValueType2, TErrorType>
{
    public TValueType1 Value1 { get; }
    public TValueType2 Value2 { get; }

    public SuccessResult(TValueType1 value1, TValueType2 value2)
    {
        Value1 = value1;
        Value2 = value2;
    }
}

public class FailureResult<TValueType1, TValueType2, TErrorType> : Result<TValueType1, TValueType2, TErrorType>
{
    public TErrorType Error { get; }

    public FailureResult(TErrorType error)
    {
        Error = error;
    }
}