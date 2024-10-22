using System;
using System.Text.RegularExpressions;

namespace Fietsenwinkel.Domain.Fietsen.Entities;
public class FietsType : IDomainType<string, FietsType>
{
    public string Value { get; }

    public FietsType(string value)
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
        new Regex(@"^[a-zA-Z0-9]+ [a-zA-Z0-9]+$").IsMatch(value);

    public static bool IsValidDomainTypeFor(string value) =>
        HasContent(value) && MatchesTypePattern(value);

    public static bool TryParse(string value, out FietsType? domainType)
    {
        if (IsValidDomainTypeFor(value))
        {
            domainType = new FietsType(value);
            return true;
        }

        domainType = default;
        return false;
    }
}
