using Fietsenwinkel.Api.Voorraden.Models.In;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Management;

namespace Fietsenwinkel.Api.Voorraden.Mappers;
internal static class FietsCreateMapper
{
    public static Result<FietsCreateModel, ErrorCodeList> Map(string filiaalId, FietsCreateInputModel inputModel)
    {
        if (inputModel == null)
        {
            return Result<FietsCreateModel, ErrorCodeList>.Fail([ErrorCodes.No_Input_Model_Provided]);
        }

        var fietsResult = Result.Combine(
            FietsInputMapper.ParseFietsType(inputModel.FietsType),
            FietsInputMapper.ParseAantalWielen(inputModel.AantalWielen),
            FietsInputMapper.ParseFrameMaat(inputModel.FrameMaat),
            FietsInputMapper.ParsePrice(inputModel.Price));

        var filiaalIdResult = FietsInputMapper.ParseFiliaalId(filiaalId);

        // helaasch gaat mijn Combine maar tot 4, dus dan maar met de hand
        return fietsResult
            .Map(
                onSuccess: (fietsType, aantalWielen, frameMaat, price) =>
                    filiaalIdResult.Map(
                        onSuccess: filiaalId =>
                            Result<FietsCreateModel, ErrorCodeList>.Succeed(
                                new FietsCreateModel(filiaalId, fietsType, aantalWielen, frameMaat, price)),
                        onFailure: Result<FietsCreateModel, ErrorCodeList>.Fail),
                onFailure: errors =>
                    filiaalIdResult.Map(
                        onSuccess: _ => Result<FietsCreateModel, ErrorCodeList>.Fail(errors),
                        onFailure: e => Result<FietsCreateModel, ErrorCodeList>.Fail(errors.Combine(e))));
    }
}
