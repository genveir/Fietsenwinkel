using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;
using System.Collections.Generic;

namespace Fietsenwinkel.Domain.Filialen.Entities;

public class FiliaalId : IDomainValueType<Guid, FiliaalId>
{
    public Guid Value { get; }

    private FiliaalId(Guid value)
    {
        if (!IsValidDomainTypeFor(value))
        {
            throw new ArgumentException("Invalid value for FiliaalId", nameof(value));
        }

        Value = value;
    }

    private static Result<Guid, ErrorCodeSet> CheckValidity(Guid guid) =>
        guid == Guid.Empty
            ? Result<Guid, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set])
            : Result<Guid, ErrorCodeSet>.Succeed(guid);

    public static bool IsValidDomainTypeFor(Guid value)
    {
        return CheckValidity(value).Switch(
            _ => true,
            _ => false);
    }

    public static Result<FiliaalId, ErrorCodeSet> Create(Guid value) =>
        CheckValidity(value).Switch(
            onSuccess: _ => Result<FiliaalId, ErrorCodeSet>.Succeed(new FiliaalId(value)),
            onFailure: Result<FiliaalId, ErrorCodeSet>.Fail);

    public static Result<FiliaalId[], ErrorCodeSet> Create(IEnumerable<Guid> values) =>
        DomainValueTypeHelper.CreateManyRecursive<Guid, FiliaalId>(values);

    public static Result<FiliaalId, ErrorCodeSet> Parse(string value) =>
        Guid.TryParse(value, out var guid) ?
            CheckValidity(guid).Switch(
                guid => Result<FiliaalId, ErrorCodeSet>.Succeed(new FiliaalId(guid)),
                Result<FiliaalId, ErrorCodeSet>.Fail)
        : Result<FiliaalId, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set]);

    public static Result<FiliaalId[], ErrorCodeSet> Parse(IEnumerable<string> values) =>
        DomainValueTypeHelper.ParseManyRecursive<Guid, FiliaalId>(values);
}