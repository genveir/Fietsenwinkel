using System;

namespace Fietsenwinkel.Shared.Results;

public static partial class Result
{
    public static Result<T1, T2, T3, TErrorType> Combine<T1, T2, T3, TErrorType>(
    Result<T1, TErrorType> r1,
    Result<T2, T3, TErrorType> r23)
    where TErrorType : ICombinable<TErrorType> =>
    r23.Switch(
        onSuccess: (s2, s3) => Combine(
            r1,
            Result<T2, TErrorType>.Succeed(s2),
            Result<T3, TErrorType>.Succeed(s3)),
        onFailure: e => Combine(
            r1,
            Result<T2, TErrorType>.Fail(e),
            Result<T3, TErrorType>.Fail(TErrorType.GetEmpty())));

    public static Result<T1, T2, T3, TErrorType> Combine<T1, T2, T3, TErrorType>(
        Result<T1, T2, TErrorType> r12,
        Result<T3, TErrorType> r3)
        where TErrorType : ICombinable<TErrorType> =>
        r12.Switch(
            onSuccess: (s1, s2) => Combine(
                Result<T1, TErrorType>.Succeed(s1),
                Result<T2, TErrorType>.Succeed(s2),
                r3),
            onFailure: e => Combine(
                Result<T1, TErrorType>.Fail(e),
                Result<T2, TErrorType>.Fail(TErrorType.GetEmpty()),
                r3));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2,
        Result<T3, T4, TErrorType> r34)
        where TErrorType : ICombinable<TErrorType> =>
        r34.Switch(
            onSuccess: (s3, s4) => Combine(
                r1,
                r2,
                Result<T3, TErrorType>.Succeed(s3),
                Result<T4, TErrorType>.Succeed(s4)),
            onFailure: e => Combine(
                r1,
                r2,
                Result<T3, TErrorType>.Fail(e),
                Result<T4, TErrorType>.Fail(TErrorType.GetEmpty())));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, T3, TErrorType> r23,
        Result<T4, TErrorType> r4)
        where TErrorType : ICombinable<TErrorType> =>
        r23.Switch(
            onSuccess: (s2, s3) => Combine(
                r1,
                Result<T2, TErrorType>.Succeed(s2),
                Result<T3, TErrorType>.Succeed(s3),
                r4),
            onFailure: e => Combine(
                r1,
                Result<T2, TErrorType>.Fail(e),
                Result<T3, TErrorType>.Fail(TErrorType.GetEmpty()),
                r4));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, T2, TErrorType> r12,
        Result<T3, TErrorType> r3,
        Result<T4, TErrorType> r4)
        where TErrorType : ICombinable<TErrorType> =>
        r12.Switch(
            onSuccess: (s1, s2) => Combine(
                Result<T1, TErrorType>.Succeed(s1),
                Result<T2, TErrorType>.Succeed(s2),
                r3,
                r4),
            onFailure: e => Combine(
                Result<T1, TErrorType>.Fail(e),
                Result<T2, TErrorType>.Fail(TErrorType.GetEmpty()),
                r3,
                r4));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, T2, TErrorType> r12,
        Result<T1, T2, TErrorType> r34)
        where TErrorType : ICombinable<TErrorType> =>
        (r12, r34) switch
        {
            (SuccessResult<T1, T2, TErrorType> s12, SuccessResult<T3, T4, TErrorType> s34) =>
                Result<T1, T2, T3, T4, TErrorType>.Succeed(s12.Value1, s12.Value2, s34.Value1, s34.Value2),
            (FailureResult<T1, T2, TErrorType> f12, FailureResult<T3, T4, TErrorType> f34) =>
                Result<T1, T2, T3, T4, TErrorType>.Fail(f12.Error.Combine(f34.Error)),
            (_, FailureResult<T3, T4, TErrorType> f34) =>
                Result<T1, T2, T3, T4, TErrorType>.Fail(f34.Error),
            (FailureResult<T1, T2, TErrorType> f12, _) =>
                Result<T1, T2, T3, T4, TErrorType>.Fail(f12.Error),
            _ => throw new NotSupportedException("Cannot combine results other than SuccessResults or FailureResults")
        };
}