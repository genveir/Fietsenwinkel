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

    private static ErrorResult<ErrorCodeSet> CheckValidity(int value) =>
        value switch
        {
            < 20 or > 100 => ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.Fiets_FrameMaat_Invalid]),
            _ => ErrorResult<ErrorCodeSet>.Succeed()
        };

    public static Result<FrameMaat, ErrorCodeSet> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<FrameMaat, ErrorCodeSet>.Succeed(new FrameMaat(value)),
            onFailure: Result<FrameMaat, ErrorCodeSet>.Fail);
}