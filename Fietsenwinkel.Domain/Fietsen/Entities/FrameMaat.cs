using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;
using System.Collections.Generic;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public class FrameMaat : IDomainValueType<int, FrameMaat>
{
    public int Value { get; }

    public static FrameMaat Default() => new(40);

    private FrameMaat(int value)
    {
        if (!IsValidDomainTypeFor(value))
        {
            throw new ArgumentException("Invalid value for FrameMaat", nameof(value));
        }

        Value = value;
    }

    private static ErrorResult<ErrorCodeSet> CheckValidity(int value) =>
        value switch
        {
            < 20 or > 100 => ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.Fiets_FrameMaat_Invalid]),
            _ => ErrorResult<ErrorCodeSet>.Succeed()
        };

    public static bool IsValidDomainTypeFor(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => true,
            onFailure: _ => false);

    public static Result<FrameMaat, ErrorCodeSet> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<FrameMaat, ErrorCodeSet>.Succeed(new FrameMaat(value)),
            onFailure: Result<FrameMaat, ErrorCodeSet>.Fail);

    public static Result<FrameMaat[], ErrorCodeSet> Create(IEnumerable<int> values) =>
        DomainValueTypeHelper.CreateManyRecursive<int, FrameMaat>(values);

    public static Result<FrameMaat, ErrorCodeSet> Parse(string value) =>
        int.TryParse(value, out var intValue) ?
            CheckValidity(intValue).Switch(
                () => Result<FrameMaat, ErrorCodeSet>.Succeed(new FrameMaat(intValue)),
                Result<FrameMaat, ErrorCodeSet>.Fail)
        : Result<FrameMaat, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set]);

    public static Result<FrameMaat[], ErrorCodeSet> Parse(IEnumerable<string> values) =>
        DomainValueTypeHelper.ParseManyRecursive<int, FrameMaat>(values);
}