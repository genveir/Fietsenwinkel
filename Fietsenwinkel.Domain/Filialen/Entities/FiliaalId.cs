using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;

namespace Fietsenwinkel.Domain.Filialen.Entities;
public class FiliaalId : IDomainValueType<Guid, FiliaalId>
{
    public Guid Value { get; }

    public FiliaalId(Guid value)
    {
        if (!IsValidDomainTypeFor(value))
        {
            throw new ArgumentException("Invalid value for FiliaalId", nameof(value));
        }

        Value = value;
    }

    private static Result<Guid, ErrorCodeSet> CheckValidity(string value) =>
        Guid.TryParse(value, out var guid) ?
            CheckValidity(guid) :
            Result<Guid, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Invalid_Format]);

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

    public static Result<FiliaalId, ErrorCodeSet> Parse(string value) =>
        CheckValidity(value).Switch(
            guid => Result<FiliaalId, ErrorCodeSet>.Succeed(new FiliaalId(guid)),
            errors => Result<FiliaalId, ErrorCodeSet>.Fail(errors));
}
