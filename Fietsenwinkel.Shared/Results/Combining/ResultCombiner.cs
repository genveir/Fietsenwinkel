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
            r1.Switch(
                onSuccess: s1 => r2.Switch(
                    onSuccess: s2 => Result<(T1, T2), TErrorType>.Succeed((s1, s2)),
                    onFailure: Result<(T1, T2), TErrorType>.Fail),
                onFailure: e1 => r2.Switch(
                    onSuccess: _ => Result<(T1, T2), TErrorType>.Fail(e1),
                    onFailure: e2 => Result<(T1, T2), TErrorType>.Fail(e1.Combine(e2))));
}