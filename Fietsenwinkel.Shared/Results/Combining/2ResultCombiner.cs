namespace Fietsenwinkel.Shared.Results;

public static partial class Result
{
    public static Result<T1, T2, T3, TErrorType> Combine<T1, T2, T3, TErrorType>(
    Result<T1, TErrorType> r1,
    Result<T2, T3, TErrorType> r23)
    where TErrorType : ICombinable<TErrorType> =>
    r23.Map(
        onSuccess: (s2, s3) => Combine(
            r1,
            Result<T2, TErrorType>.Succeed(s2),
            Result<T3, TErrorType>.Succeed(s3)),
        onFailure: e => Combine(
            r1,
            Result<T2, TErrorType>.Fail(e),
            Result<T3, TErrorType>.Fail(TErrorType.GetNeutral())));

    public static Result<T1, T2, T3, TErrorType> Combine<T1, T2, T3, TErrorType>(
        Result<T1, T2, TErrorType> r12,
        Result<T3, TErrorType> r3)
        where TErrorType : ICombinable<TErrorType> =>
        r12.Map(
            onSuccess: (s1, s2) => Combine(
                Result<T1, TErrorType>.Succeed(s1),
                Result<T2, TErrorType>.Succeed(s2),
                r3),
            onFailure: e => Combine(
                Result<T1, TErrorType>.Fail(e),
                Result<T2, TErrorType>.Fail(TErrorType.GetNeutral()),
                r3));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2,
        Result<T3, T4, TErrorType> r34)
        where TErrorType : ICombinable<TErrorType> =>
        r34.Map(
            onSuccess: (s3, s4) => Combine(
                r1,
                r2,
                Result<T3, TErrorType>.Succeed(s3),
                Result<T4, TErrorType>.Succeed(s4)),
            onFailure: e => Combine(
                r1,
                r2,
                Result<T3, TErrorType>.Fail(e),
                Result<T4, TErrorType>.Fail(TErrorType.GetNeutral())));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, T3, TErrorType> r23,
        Result<T4, TErrorType> r4)
        where TErrorType : ICombinable<TErrorType> =>
        r23.Map(
            onSuccess: (s2, s3) => Combine(
                r1,
                Result<T2, TErrorType>.Succeed(s2),
                Result<T3, TErrorType>.Succeed(s3),
                r4),
            onFailure: e => Combine(
                r1,
                Result<T2, TErrorType>.Fail(e),
                Result<T3, TErrorType>.Fail(TErrorType.GetNeutral()),
                r4));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, T2, TErrorType> r12,
        Result<T3, TErrorType> r3,
        Result<T4, TErrorType> r4)
        where TErrorType : ICombinable<TErrorType> =>
        r12.Map(
            onSuccess: (s1, s2) => Combine(
                Result<T1, TErrorType>.Succeed(s1),
                Result<T2, TErrorType>.Succeed(s2),
                r3,
                r4),
            onFailure: e => Combine(
                Result<T1, TErrorType>.Fail(e),
                Result<T2, TErrorType>.Fail(TErrorType.GetNeutral()),
                r3,
                r4));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, T2, TErrorType> r12,
        Result<T3, T4, TErrorType> r34)
        where TErrorType : ICombinable<TErrorType> =>
        r12.Map(
            onSuccess: (s1, s2) => r34.Map(
                onSuccess: (s3, s4) => Result<T1, T2, T3, T4, TErrorType>.Succeed(s1, s2, s3, s4),
                onFailure: Result<T1, T2, T3, T4, TErrorType>.Fail),
            onFailure: e12 => r34.Map(
                onSuccess: (_, _) => Result<T1, T2, T3, T4, TErrorType>.Fail(e12),
                onFailure: e34 => Result<T1, T2, T3, T4, TErrorType>.Fail(e12.Combine(e34))));
}