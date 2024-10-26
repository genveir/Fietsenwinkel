using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Domain.Fietsen.Entities;

public class AantalWielen : IDomainValueType<int, AantalWielen>
{
    public int Value { get; }

    public static AantalWielen Default() => new(2);

    private AantalWielen(int value)
    {
        Value = value;
    }

    private static ErrorResult<ErrorCodeSet> CheckValidity(int value) =>
        value switch
        {
            < 1 => ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.Fiets_Has_No_Wheels]),
            > 3 => ErrorResult<ErrorCodeSet>.Fail([ErrorCodes.Fiets_Has_Too_Many_Wheels]),
            _ => ErrorResult<ErrorCodeSet>.Succeed()
        };

    public static Result<AantalWielen, ErrorCodeSet> Create(int value) =>
        CheckValidity(value).Switch(
            onSuccess: () => Result<AantalWielen, ErrorCodeSet>.Succeed(new AantalWielen(value)),
            onFailure: Result<AantalWielen, ErrorCodeSet>.Fail);
}