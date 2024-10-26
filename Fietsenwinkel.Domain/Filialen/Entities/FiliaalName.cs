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

    private static ErrorResult<ErrorCodeSet> CheckValidity(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.FiliaalName_Value_NotSet])
            : ErrorResult<ErrorCodeSet>.Succeed();

    public static Result<FiliaalName, ErrorCodeSet> Create(string value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<FiliaalName, ErrorCodeSet>.Succeed(new FiliaalName(value)),
            onFailure: Result<FiliaalName, ErrorCodeSet>.Fail);
}