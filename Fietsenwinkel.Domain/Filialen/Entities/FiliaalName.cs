using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;
using System.Collections.Generic;

namespace Fietsenwinkel.Domain.Filialen.Entities;

public class FiliaalName : IDomainValueType<string, FiliaalName>
{
    public string Value { get; }

    public static FiliaalName Default() => new("No Name Set");

    private FiliaalName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("filiaal name can not be empty", nameof(value));
        }

        Value = value;
    }

    private static Result<string, ErrorCodeSet> CheckValidity(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? Result<string, ErrorCodeSet>.Fail([ErrorCodes.FiliaalName_Value_NotSet])
            : Result<string, ErrorCodeSet>.Succeed(value);

    public static bool IsValidDomainTypeFor(string value) =>
        CheckValidity(value).Switch(
            _ => true,
            _ => false);

    public static Result<FiliaalName, ErrorCodeSet> Create(string value) =>
        CheckValidity(value).Switch(
            onSuccess: _ => Result<FiliaalName, ErrorCodeSet>.Succeed(new FiliaalName(value)),
            onFailure: Result<FiliaalName, ErrorCodeSet>.Fail);

    public static Result<FiliaalName[], ErrorCodeSet> Create(IEnumerable<string> values) =>
        DomainValueTypeHelper.CreateManyRecursive<string, FiliaalName>(values);

    public static Result<FiliaalName, ErrorCodeSet> Parse(string value) =>
        CheckValidity(value).Switch(
            onSuccess: _ => Result<FiliaalName, ErrorCodeSet>.Succeed(new FiliaalName(value)),
            onFailure: Result<FiliaalName, ErrorCodeSet>.Fail);

    public static Result<FiliaalName[], ErrorCodeSet> Parse(IEnumerable<string> values) =>
        DomainValueTypeHelper.ParseManyRecursive<string, FiliaalName>(values);
}