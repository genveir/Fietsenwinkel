using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public class FrameMaat : IDomainValueType<int, FrameMaat>
{
    public int Value { get; }

    private FrameMaat(int value)
    {
        Value = value;
    }

    private static ErrorResult<ErrorCodeList> CheckValidity(int value) =>
        value switch
        {
            < 20 or > 100 => ErrorResult<ErrorCodeList>.Fail([ErrorCodes.FrameMaat_Invalid]),
            _ => ErrorResult<ErrorCodeList>.Succeed()
        };

    public static Result<FrameMaat, ErrorCodeList> Create(int value) =>
        CheckValidity(value).Map(
            onSuccess: () => Result<FrameMaat, ErrorCodeList>.Succeed(new FrameMaat(value)),
            onFailure: Result<FrameMaat, ErrorCodeList>.Fail);
}