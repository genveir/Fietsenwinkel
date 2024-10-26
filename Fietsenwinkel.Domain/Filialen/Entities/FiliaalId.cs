using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain.Filialen.Entities;

public class FiliaalId : IDomainValueType<int, FiliaalId>
{
    public int Value { get; }

    public static FiliaalId Default() => new(-1);

    private FiliaalId(int value)
    {
        Value = value;
    }

    private static ErrorResult<ErrorCodeSet> CheckValidity(int value) =>
        value == 0
            ? ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set])
            : ErrorResult<ErrorCodeSet>.Succeed();

    public static Result<FiliaalId, ErrorCodeSet> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<FiliaalId, ErrorCodeSet>.Succeed(new FiliaalId(value)),
            onFailure: Result<FiliaalId, ErrorCodeSet>.Fail);

    public static Result<FiliaalId, ErrorCodeSet> Parse(string value) =>
        int.TryParse(value, out var intValue) ?
            Create(intValue) :
            Result<FiliaalId, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Invalid_Format]);
}