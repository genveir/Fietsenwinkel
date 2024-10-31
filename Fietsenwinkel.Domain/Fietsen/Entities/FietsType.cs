using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System.Text.RegularExpressions;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public partial class FietsType : IDomainValueType<string, FietsType>
{
    public string Value { get; }

    private FietsType(string value)
    {
        Value = value;
    }

    private static bool HasContent(string value) =>
        !string.IsNullOrWhiteSpace(value);

    private static bool MatchesTypePattern(string value) =>
        TypePattern().IsMatch(value);

    private static ErrorResult<ErrorCodeList> CheckValidity(string value)
    {
        var errors = new ErrorCodeList();

        if (!HasContent(value))
        {
            errors.Add(ErrorCodes.Fietstype_Value_Not_Set);
        }

        if (!MatchesTypePattern(value))
        {
            errors.Add(ErrorCodes.Fietstype_Invalid_Format);
        }

        if (errors.Count != 0)
        {
            return ErrorResult<ErrorCodeList>.Fail(errors);
        }
        return ErrorResult<ErrorCodeList>.Succeed();
    }

    public static Result<FietsType, ErrorCodeList> Create(string value) =>
        CheckValidity(value)
            .Switch(
                onSuccess: () => Result<FietsType, ErrorCodeList>.Succeed(new FietsType(value)),
                onFailure: Result<FietsType, ErrorCodeList>.Fail);

    [GeneratedRegex(@"^[a-zA-Z0-9]+ [a-zA-Z0-9]+$")]
    protected static partial Regex TypePattern();
}