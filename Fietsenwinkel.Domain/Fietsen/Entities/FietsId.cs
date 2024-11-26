using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain.Fietsen.Entities;
public class FietsId : IDomainValueType<int, FietsId>
{
    public int Value { get; }

    private FietsId(int value) => Value = value;

    private static ErrorResult<ErrorCodeList> CheckValidity(int value) =>
        value switch
        {
            0 => ErrorResult<ErrorCodeList>.Fail([ErrorCodes.FietsId_Value_Not_Set]),
            _ => ErrorResult<ErrorCodeList>.Succeed()
        };

    public static Result<FietsId, ErrorCodeList> Create(int value) =>
        CheckValidity(value).Map(
            onSuccess: () => Result<FietsId, ErrorCodeList>.Succeed(new FietsId(value)),
            onFailure: Result<FietsId, ErrorCodeList>.Fail);

    public static Result<FietsId, ErrorCodeList> Parse(string value) =>
        int.TryParse(value, out var intValue) ?
            Create(intValue) :
            Result<FietsId, ErrorCodeList>.Fail([ErrorCodes.FietsId_Invalid_Format]);
}
