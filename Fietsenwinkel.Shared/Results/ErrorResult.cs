﻿using System;

namespace Fietsenwinkel.Shared.Results;

public abstract class ErrorResult<TErrorType>
{
    public static ErrorResult<TErrorType> Succeed() => new ErrorSuccessResult<TErrorType>();

    public static ErrorResult<TErrorType> Fail(TErrorType value) => new ErrorFailureResult<TErrorType>(value);

    public TReturnType Switch<TReturnType>(
        Func<TReturnType> onSuccess,
        Func<TErrorType, TReturnType> onFailure)
    {
        return this switch
        {
            ErrorSuccessResult<TErrorType> success => onSuccess(),
            ErrorFailureResult<TErrorType> failure => onFailure(failure.Error),
            _ => throw new InvalidOperationException("Unknown result type")
        };
    }
}

public class ErrorSuccessResult<TErrorType> : ErrorResult<TErrorType>
{ }

public class ErrorFailureResult<TErrorType> : ErrorResult<TErrorType>
{
    public TErrorType Error { get; }

    public ErrorFailureResult(TErrorType error)
    {
        Error = error;
    }
}