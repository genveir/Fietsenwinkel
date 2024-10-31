using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain.Filialen.Entities;

public class FiliaalId : IDomainValueType<int, FiliaalId>
{
    public int Value { get; }

    private FiliaalId(int value)
    {
        Value = value;
    }

    private static ErrorResult<ErrorCodeList> CheckValidity(int value) =>
        value == 0
            ? ErrorResult<ErrorCodeList>.Fail([ErrorCodes.FiliaalId_Value_Not_Set])
            : ErrorResult<ErrorCodeList>.Succeed();

    public static Result<FiliaalId, ErrorCodeList> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<FiliaalId, ErrorCodeList>.Succeed(new FiliaalId(value)),
            onFailure: Result<FiliaalId, ErrorCodeList>.Fail);

    public static Result<FiliaalId, ErrorCodeList> Parse(string value) =>
        int.TryParse(value, out var intValue) ?
            Create(intValue) :
            Result<FiliaalId, ErrorCodeList>.Fail([ErrorCodes.FiliaalId_Invalid_Format]);
}