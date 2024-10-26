using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;
using System.Collections.Generic;

namespace Fietsenwinkel.Domain.Filialen.Entities;

public class FiliaalId : IDomainValueType<int, FiliaalId>
{
    public int Value { get; }

    public static FiliaalId Default() => new(-1);

    private FiliaalId(int value)
    {
        if (!IsValidDomainTypeFor(value))
        {
            throw new ArgumentException("Invalid value for FiliaalId", nameof(value));
        }

        Value = value;
    }

    private static Result<int, ErrorCodeSet> CheckValidity(int value) =>
        value == 0
            ? Result<int, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set])
            : Result<int, ErrorCodeSet>.Succeed(value);

    public static bool IsValidDomainTypeFor(int value) =>
        CheckValidity(value).Switch(
            _ => true,
            _ => false);

    public static Result<FiliaalId, ErrorCodeSet> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: _ => Result<FiliaalId, ErrorCodeSet>.Succeed(new FiliaalId(value)),
            onFailure: Result<FiliaalId, ErrorCodeSet>.Fail);

    public static Result<FiliaalId[], ErrorCodeSet> Create(IEnumerable<int> values) =>
        DomainValueTypeHelper.CreateManyRecursive<int, FiliaalId>(values);

    public static Result<FiliaalId, ErrorCodeSet> Parse(string value) =>
        int.TryParse(value, out var intValue) ?
            CheckValidity(intValue).Switch(
                guid => Result<FiliaalId, ErrorCodeSet>.Succeed(new FiliaalId(guid)),
                Result<FiliaalId, ErrorCodeSet>.Fail)
        : Result<FiliaalId, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set]);

    public static Result<FiliaalId[], ErrorCodeSet> Parse(IEnumerable<string> values) =>
        DomainValueTypeHelper.ParseManyRecursive<int, FiliaalId>(values);
}