namespace Fietsenwinkel.Shared.Results;

public static partial class Result
{
    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, T3, T4, TErrorType> r234)
        where TErrorType : ICombinable<TErrorType> =>
        r234.Switch(
            onSuccess: (s2, s3, s4) => Combine(
                r1,
                Result<T2, TErrorType>.Succeed(s2),
                Result<T3, TErrorType>.Succeed(s3),
                Result<T4, TErrorType>.Succeed(s4)),
            onFailure: e => Combine(
                r1,
                Result<T2, TErrorType>.Fail(e),
                Result<T3, TErrorType>.Fail(TErrorType.GetNeutral()),
                Result<T4, TErrorType>.Fail(TErrorType.GetNeutral())));

    public static Result<T1, T2, T3, T4, TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, T2, T3, TErrorType> r123,
        Result<T4, TErrorType> r4)
        where TErrorType : ICombinable<TErrorType> =>
        r123.Switch(
            onSuccess: (s1, s2, s3) => Combine(
                Result<T1, TErrorType>.Succeed(s1),
                Result<T2, TErrorType>.Succeed(s2),
                Result<T3, TErrorType>.Succeed(s3),
                r4),
            onFailure: e => Combine(
                Result<T1, TErrorType>.Fail(e),
                Result<T2, TErrorType>.Fail(TErrorType.GetNeutral()),
                Result<T3, TErrorType>.Fail(TErrorType.GetNeutral()),
                r4));
}