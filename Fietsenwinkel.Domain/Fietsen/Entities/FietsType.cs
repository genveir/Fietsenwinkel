using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fietsenwinkel.Domain.Fietsen.Entities;
public partial class FietsType : IDomainValueType<string, FietsType>
{
    public string Value { get; }

    private FietsType(string value)
    {
        if (!HasContent(value))
        {
            throw new ArgumentException("FietsType can not be empty", nameof(value));
        }

        if (!MatchesTypePattern(value))
        {
            throw new ArgumentException("FietsType must be in the format 'Merknaam Typenaam'", nameof(value));
        }

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

    public static bool IsValidDomainTypeFor(string value) =>
        CheckValidity(value).Switch(
            () => true,
            _ => false);

    public static Result<FietsType, ErrorCodeSet> Parse(string value) =>
        CheckValidity(value)
            .Switch(
                () => Result<FietsType, ErrorCodeSet>.Succeed(new FietsType(value)),
                Result<FietsType, ErrorCodeSet>.Fail);

    public static Result<FietsType[], ErrorCodeSet> Parse(IEnumerable<string> values) =>
        ParseManyRecursive(values.ToArray(), 0, new List<FietsType>(), new List<ErrorCodes>());

    private static Result<FietsType[], ErrorCodeSet> ParseManyRecursive(string[] values, int index, List<FietsType> fietsTypes, List<ErrorCodes> errors)
    {
        if (index == values.Length)
        {
            return errors.Count == 0 ?
                Result<FietsType[], ErrorCodeSet>.Succeed(fietsTypes.ToArray()) :
                Result<FietsType[], ErrorCodeSet>.Fail(new ErrorCodeSet(errors));
        }

        return Parse(values[index])
            .Switch(
                onSuccess: v => ParseManyRecursive(values, index + 1, fietsTypes.Append(v).ToList(), errors),
                onFailure: v => ParseManyRecursive(values, index + 1, fietsTypes, errors.Union(v).ToList()));
    }


    [GeneratedRegex(@"^[a-zA-Z0-9]+ [a-zA-Z0-9]+$")]
    protected static partial Regex TypePattern();
}
