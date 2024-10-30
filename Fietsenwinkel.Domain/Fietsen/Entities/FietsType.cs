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

    private static ErrorResult<ErrorCodeSet> CheckValidity(string value)
    {
        var errors = new ErrorCodeSet();

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
            return ErrorResult<ErrorCodeSet>.Fail(errors);
        }
        return ErrorResult<ErrorCodeSet>.Succeed();
    }

    public static Result<FietsType, ErrorCodeSet> Create(string value) =>
        CheckValidity(value)
            .Switch(
                () => Result<FietsType, ErrorCodeSet>.Succeed(new FietsType(value)),
                Result<FietsType, ErrorCodeSet>.Fail);

    [GeneratedRegex(@"^[a-zA-Z0-9]+ [a-zA-Z0-9]+$")]
    protected static partial Regex TypePattern();
}