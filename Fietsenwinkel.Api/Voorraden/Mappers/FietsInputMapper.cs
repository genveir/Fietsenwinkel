using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;

namespace Fietsenwinkel.Api.Voorraden.Mappers;

public static class FietsInputMapper
{
    internal static Result<FiliaalId, ErrorCodeList> ParseFiliaalId(string? filiaalId)
    {
        if (filiaalId == null)
        {
            return Result<FiliaalId, ErrorCodeList>.Fail([ErrorCodes.FiliaalId_Value_Not_Set]);
        }

        return FiliaalId.Parse(filiaalId);
    }

    internal static Result<FietsId, ErrorCodeList> ParseFietsId(string? fietsId)
    {
        if (fietsId == null)
        {
            return Result<FietsId, ErrorCodeList>.Fail([ErrorCodes.FietsId_Value_Not_Set]);
        }

        return FietsId.Parse(fietsId);
    }

    internal static Result<FietsType, ErrorCodeList> ParseFietsType(string? fietsType)
    {
        if (string.IsNullOrWhiteSpace(fietsType))
        {
            return Result<FietsType, ErrorCodeList>.Fail([ErrorCodes.FietsType_Value_Not_Set]);
        }

        return FietsType.Create(fietsType);
    }

    internal static Result<AantalWielen, ErrorCodeList> ParseAantalWielen(int? aantalWielen)
    {
        if (aantalWielen == null)
        {
            return Result<AantalWielen, ErrorCodeList>.Fail([ErrorCodes.AantalWielen_Value_Not_Set]);
        }

        return AantalWielen.Create(aantalWielen.Value);
    }

    internal static Result<FrameMaat, ErrorCodeList> ParseFrameMaat(int? frameMaat)
    {
        if (frameMaat == null)
        {
            return Result<FrameMaat, ErrorCodeList>.Fail([ErrorCodes.FrameMaat_Value_Not_Set]);
        }

        return FrameMaat.Create(frameMaat.Value);
    }

    internal static Result<Money, ErrorCodeList> ParsePrice(int? price)
    {
        if (price == null)
        {
            return Result<Money, ErrorCodeList>.Fail([ErrorCodes.Price_Value_Not_Set]);
        }

        return Money.Create(price.Value);
    }
}
