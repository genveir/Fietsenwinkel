using System;

namespace Fietsenwinkel.Shared.Results;

public static partial class Result
{
    public static Result<T1, T2, TErrorType> Combine<T1, T2, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2)
        where TErrorType : ICombinable<TErrorType> =>
        CombineToTuple(r1, r2).Switch(
            onSuccess: vt =>
            {
                var (s1, s2) = vt;

                return Result<T1, T2, TErrorType>.Succeed(s1, s2);
            },
            onFailure: Result<T1, T2, TErrorType>.Fail);

    public static Result<T1, T2, T3, TErrorType> Combine<T1, T2, T3, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2,
        Result<T3, TErrorType> r3)
        where TErrorType : ICombinable<TErrorType> =>
        CombineToTuple(
            CombineToTuple(r1, r2),
            r3).Switch(
                onSuccess: threetuple =>
                {
                    var ((s1, s2), s3) = threetuple;

                    return Result<T1, T2, T3, TErrorType>.Succeed(s1, s2, s3);
                },
                onFailure: Result<T1, T2, T3, TErrorType>.Fail);

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2,
        Result<T3, TErrorType> r3,
        Result<T4, TErrorType> r4)
        where TErrorType : ICombinable<TErrorType> =>
        CombineToTuple(
            CombineToTuple(r1, r2),
            CombineToTuple(r3, r4))
                .Switch(
                    onSuccess: fourTuple =>
                    {
                        var ((s1, s2), (s3, s4)) = fourTuple;

                        return Result<T1, T2, T3, T4, TErrorType>.Succeed(s1, s2, s3, s4);
                    },
                    onFailure: Result<T1, T2, T3, T4, TErrorType>.Fail);

    private static Result<(T1, T2), TErrorType> CombineToTuple<T1, T2, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2)
        where TErrorType : ICombinable<TErrorType> =>
        (r1, r2) switch
        {
            (SuccessResult<T1, TErrorType> s1, SuccessResult<T2, TErrorType> s2) => Result<(T1, T2), TErrorType>.Succeed((s1.Value, s2.Value)),
            (FailureResult<T1, TErrorType> f1, FailureResult<T2, TErrorType> f2) => Result<(T1, T2), TErrorType>.Fail(f1.Error.Combine(f2.Error)),
            (_, FailureResult<T2, TErrorType> f2) => Result<(T1, T2), TErrorType>.Fail(f2.Error),
            (FailureResult<T1, TErrorType> f1, _) => Result<(T1, T2), TErrorType>.Fail(f1.Error),

            _ => throw new NotSupportedException("Cannot combine results other than SuccessResults or FailureResults")
        };
}