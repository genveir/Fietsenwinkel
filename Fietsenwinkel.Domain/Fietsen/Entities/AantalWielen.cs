using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;
using System.Collections.Generic;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public class AantalWielen : IDomainValueType<int, AantalWielen>
{
    public int Value { get; }

    public static AantalWielen Default() => new(2);

    private AantalWielen(int value)
    {
        if (!IsValidDomainTypeFor(value))
        {
            throw new ArgumentException("Invalid value for AantalWielen", nameof(value));
        }

        Value = value;
    }

    private static ErrorResult<ErrorCodeSet> CheckValidity(int value) =>
        value switch
        {
            < 1 => ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.Fiets_Has_No_Wheels]),
            > 3 => ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.Fiets_Has_Too_Many_Wheels]),
            _ => ErrorResult<ErrorCodeSet>.Succeed()
        };

    public static bool IsValidDomainTypeFor(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => true,
            onFailure: _ => false);

    public static Result<AantalWielen, ErrorCodeSet> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<AantalWielen, ErrorCodeSet>.Succeed(new AantalWielen(value)),
            onFailure: Result<AantalWielen, ErrorCodeSet>.Fail);

    public static Result<AantalWielen[], ErrorCodeSet> Create(IEnumerable<int> values) =>
        DomainValueTypeHelper.CreateManyRecursive<int, AantalWielen>(values);

    public static Result<AantalWielen, ErrorCodeSet> Parse(string value) =>
        int.TryParse(value, out var intValue) ?
            CheckValidity(intValue).Switch(
                () => Result<AantalWielen, ErrorCodeSet>.Succeed(new AantalWielen(intValue)),
                Result<AantalWielen, ErrorCodeSet>.Fail)
        : Result<AantalWielen, ErrorCodeSet>.Fail([ErrorCodes.FiliaalId_Value_Not_Set]);

    public static Result<AantalWielen[], ErrorCodeSet> Parse(IEnumerable<string> values) =>
        DomainValueTypeHelper.ParseManyRecursive<int, AantalWielen>(values);
}