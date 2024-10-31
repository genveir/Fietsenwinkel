using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public class AantalWielen : IDomainValueType<int, AantalWielen>
{
    public int Value { get; }

    private AantalWielen(int value)
    {
        Value = value;
    }

    private static ErrorResult<ErrorCodeList> CheckValidity(int value) =>
        value switch
        {
            < 1 => ErrorResult<ErrorCodeList>.Fail([ErrorCodes.Fiets_Has_No_Wheels]),
            > 3 => ErrorResult<ErrorCodeList>.Fail([ErrorCodes.Fiets_Has_Too_Many_Wheels]),
            _ => ErrorResult<ErrorCodeList>.Succeed()
        };

    public static Result<AantalWielen, ErrorCodeList> Create(int value) =>
        CheckValidity(value).Map(
            onSuccess: () => Result<AantalWielen, ErrorCodeList>.Succeed(new AantalWielen(value)),
            onFailure: Result<AantalWielen, ErrorCodeList>.Fail);
}