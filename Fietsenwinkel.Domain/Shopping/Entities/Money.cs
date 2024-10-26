using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;

namespace Fietsenwinkel.Domain.Shopping.Entities;

public class Money : IDomainValueType<int, Money>
{
    public int Value { get; }

    public static Money Default() => new(-1);

    private Money(int value)
    {
        CheckValidity(value).Switch(
            onSuccess: () => { },
            onFailure: _ => throw new ArgumentException("Invalid value for Money", nameof(value)));

        Value = value;
    }

    private static ErrorResult<ErrorCodeSet> CheckValidity(int value) =>
        value == 0
            ? ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.Money_Value_Not_Set])
            : ErrorResult<ErrorCodeSet>.Succeed();

    public static Result<Money, ErrorCodeSet> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<Money, ErrorCodeSet>.Succeed(new Money(value)),
            onFailure: Result<Money, ErrorCodeSet>.Fail);
}