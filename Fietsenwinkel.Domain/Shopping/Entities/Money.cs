using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System;

namespace Fietsenwinkel.Domain.Shopping.Entities;

public class Money : IDomainValueType<int, Money>
{
    public int Value { get; }

    private Money(int value)
    {
        CheckValidity(value).Act(
            onSuccess: () => { },
            onFailure: _ => throw new ArgumentException("Invalid value for Money", nameof(value)));

        Value = value;
    }

    private static ErrorResult<ErrorCodeList> CheckValidity(int value) =>
        value == 0
            ? ErrorResult<ErrorCodeList>.Fail([ErrorCodes.Money_Value_Not_Set])
            : ErrorResult<ErrorCodeList>.Succeed();

    public static Result<Money, ErrorCodeList> Create(int value) =>
        CheckValidity(value).Map(
            onSuccess: () => Result<Money, ErrorCodeList>.Succeed(new Money(value)),
            onFailure: Result<Money, ErrorCodeList>.Fail);
}