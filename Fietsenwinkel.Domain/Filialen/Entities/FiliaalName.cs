using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain.Filialen.Entities;

public class FiliaalName : IDomainValueType<string, FiliaalName>
{
    public string Value { get; }

    public static FiliaalName Default() => new("No Name Set");

    private FiliaalName(string value)
    {
        Value = value;
    }

    private static ErrorResult<ErrorCodeList> CheckValidity(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? ErrorResult<ErrorCodeList>.Fail([ErrorCodes.FiliaalName_Value_NotSet])
            : ErrorResult<ErrorCodeList>.Succeed();

    public static Result<FiliaalName, ErrorCodeList> Create(string value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<FiliaalName, ErrorCodeList>.Succeed(new FiliaalName(value)),
            onFailure: Result<FiliaalName, ErrorCodeList>.Fail);
}