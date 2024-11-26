using Fietsenwinkel.Api.Voorraden.Models.In;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Management;

namespace Fietsenwinkel.Api.Voorraden.Mappers;

public static class FietsUpdateMapper
{
    public static Result<FietsUpdateModel, ErrorCodeList> Map(string filiaalId, string fietsId, FietsUpdateInputModel inputModel)
    {
        var idsResult = Result.Combine(
            FietsInputMapper.ParseFiliaalId(filiaalId),
            FietsInputMapper.ParseFietsId(fietsId));

        var fietsResult = Result.Combine(
            FietsInputMapper.ParseFietsType(inputModel.FietsType),
            FietsInputMapper.ParseAantalWielen(inputModel.AantalWielen),
            FietsInputMapper.ParseFrameMaat(inputModel.FrameMaat),
            FietsInputMapper.ParsePrice(inputModel.Price));

        // helaasch gaat mijn Combine maar tot 4, dus dan maar met de hand
        return idsResult
            .Map(
                onSuccess: (filiaalId, fietsId) =>
                    fietsResult.Map(
                        onSuccess: (fietsType, aantalWielen, frameMaat, price) =>
                            Result<FietsUpdateModel, ErrorCodeList>.Succeed(
                                new FietsUpdateModel(filiaalId, fietsId, fietsType, aantalWielen, frameMaat, price)),
                        onFailure: Result<FietsUpdateModel, ErrorCodeList>.Fail),
                onFailure: errors =>
                    fietsResult.Map(
                            onSuccess: (_, _, _, _) => Result<FietsUpdateModel, ErrorCodeList>.Fail(errors),
                            onFailure: e => Result<FietsUpdateModel, ErrorCodeList>.Fail(errors.Combine(e))));
    }
}
