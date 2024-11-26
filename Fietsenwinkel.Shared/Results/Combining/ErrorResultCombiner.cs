using System.Linq;

namespace Fietsenwinkel.Shared.Results;
public static partial class Result
{
    public static ErrorResult<TErrorType> Combine<TErrorType>(params ErrorResult<TErrorType>[] results) where TErrorType : ICombinable<TErrorType> =>
        results.Aggregate(CombineTwo);

    private static ErrorResult<TErrorType> CombineTwo<TErrorType>(
        ErrorResult<TErrorType> r1,
        ErrorResult<TErrorType> r2)
        where TErrorType : ICombinable<TErrorType> =>
        r1.Map(
            onSuccess: () => r2.Map(
                onSuccess: () => ErrorResult<TErrorType>.Succeed(),
                onFailure: _ => r2),
            onFailure: e1 => r2.Map(
                onSuccess: () => r1,
                onFailure: e2 => ErrorResult<TErrorType>.Fail(e1.Combine(e2))));
}
