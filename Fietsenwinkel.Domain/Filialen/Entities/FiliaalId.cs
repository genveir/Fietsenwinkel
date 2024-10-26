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

    private static ErrorResult<ErrorCodeSet> CheckValidity(int value) =>
        value == 0
            ? ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set])
            : ErrorResult<ErrorCodeSet>.Succeed();

    public static bool IsValidDomainTypeFor(int value) =>
        CheckValidity(value).Switch(
            () => true,
            _ => false);

    public static Result<FiliaalId, ErrorCodeSet> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<FiliaalId, ErrorCodeSet>.Succeed(new FiliaalId(value)),
            onFailure: Result<FiliaalId, ErrorCodeSet>.Fail);

    public static Result<FiliaalId[], ErrorCodeSet> Create(IEnumerable<int> values) =>
        DomainValueTypeHelper.CreateManyRecursive<int, FiliaalId>(values);

    public static Result<FiliaalId, ErrorCodeSet> Parse(string value) =>
        int.TryParse(value, out var intValue) ?
            CheckValidity(intValue).Switch(
                () => Result<FiliaalId, ErrorCodeSet>.Succeed(new FiliaalId(intValue)),
                Result<FiliaalId, ErrorCodeSet>.Fail)
        : Result<FiliaalId, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set]);

    public static Result<FiliaalId[], ErrorCodeSet> Parse(IEnumerable<string> values) =>
        DomainValueTypeHelper.ParseManyRecursive<int, FiliaalId>(values);
}