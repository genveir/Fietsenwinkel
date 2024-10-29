namespace Fietsenwinkel.Shared.Results;

public interface ICombinable<TCombinableType>
{
    TCombinableType Combine(TCombinableType combinable);
}

public static class Result
{
    public static Result<(T1, T2), TErrorType> Combine<T1, T2, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2)
        where TErrorType : ICombinable<TErrorType> =>
        CombineTwo(r1, r2);

    public static Result<(T1, T2, T3), TErrorType> Combine<T1, T2, T3, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2,
        Result<T3, TErrorType> r3)
        where TErrorType : ICombinable<TErrorType> =>
        Combine(
            Combine(r1, r2),
            r3).Switch(
                onSuccess: threetuple =>
                {
                    var ((s1, s2), s3) = threetuple;

                    return Result<(T1, T2, T3), TErrorType>.Succeed((s1, s2, s3));
                },
                onFailure: Result<(T1, T2, T3), TErrorType>.Fail);

    public static Result<(T1, T2, T3, T4), TErrorType> Combine<T1, T2, T3, T4, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2,
        Result<T3, TErrorType> r3,
        Result<T4, TErrorType> r4)
        where TErrorType : ICombinable<TErrorType> =>
        Combine(
            Combine(r1, r2),
            Combine(r3, r4))
                .Switch(
                    onSuccess: fourTuple =>
                    {
                        var ((s1, s2), (s3, s4)) = fourTuple;

                        return Result<(T1, T2, T3, T4), TErrorType>.Succeed((s1, s2, s3, s4));
                    },
                    onFailure: Result<(T1, T2, T3, T4), TErrorType>.Fail);

    public static Result<(T1, T2, T3, T4, T5), TErrorType> Combine<T1, T2, T3, T4, T5, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2,
        Result<T3, TErrorType> r3,
        Result<T4, TErrorType> r4,
        Result<T5, TErrorType> r5)
        where TErrorType : ICombinable<TErrorType> =>
        Combine(
            Combine(r1, r2, r3, r4),
            r5).Switch(
                onSuccess: fiveTuple =>
                {
                    var ((s1, s2, s3, s4), s5) = fiveTuple;

                    return Result<(T1, T2, T3, T4, T5), TErrorType>.Succeed((s1, s2, s3, s4, s5));
                },
                onFailure: Result<(T1, T2, T3, T4, T5), TErrorType>.Fail);

    private static Result<(T1, T2), TErrorType> CombineTwo<T1, T2, TErrorType>(
        Result<T1, TErrorType> r1,
        Result<T2, TErrorType> r2)
        where TErrorType : ICombinable<TErrorType>
    {
        T1? s1 = default;
        T2? s2 = default;

        bool errorsHaveBeenSet = false;
        TErrorType? errors = default;

        r1.Switch(
            onSuccess: s => s1 = s,
            onFailure: e =>
            {
                errors = e;
                errorsHaveBeenSet = true;
            });

        r2.Switch(
            onSuccess: s => s2 = s,
            onFailure: e =>
            {
                errors = errorsHaveBeenSet ? e : errors!.Combine(e);
                errorsHaveBeenSet = true;
            });

        if (errorsHaveBeenSet)
        {
            return Result<(T1, T2), TErrorType>.Fail(errors!);
        }
        else
        {
            return Result<(T1, T2), TErrorType>.Succeed((s1!, s2!));
        }
    }
}