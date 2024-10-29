using Fietsenwinkel.Api.Shopping.Models.In;
using Fietsenwinkel.Api.Shopping.Models.Out;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Shopping;

namespace Fietsenwinkel.Api.Shopping.Mappers;

public static class FietsSearchMapper
{
    public static Result<FietsSearchQuery, ErrorCodeSet> Map(FietsSearchInputModel model)
    {
        if (model == null)
        {
            return Result<FietsSearchQuery, ErrorCodeSet>.Fail([ErrorCodes.No_Input_Model_Provided]);
        }

        return Result.Combine(
            MapKlant(model),
            MapFietsType(model)).Switch(
            onSuccess: vt =>
            {
                var (klant, fietsType) = vt;

                var fietsSearchQuery = new FietsSearchQuery(klant, fietsType);

                return Result<FietsSearchQuery, ErrorCodeSet>.Succeed(fietsSearchQuery);
            },
            onFailure: Result<FietsSearchQuery, ErrorCodeSet>.Fail);
    }

    private static Result<Klant, ErrorCodeSet> MapKlant(FietsSearchInputModel model)
    {
        var errors = new ErrorCodeSet();

        if (model.UserHeight == null)
        {
            errors.Add(ErrorCodes.User_Height_Not_Set);
        }
        else if (model.UserHeight < 150)
        {
            errors.Add(ErrorCodes.User_Too_Short);
        }
        else if (model.UserHeight > 210)
        {
            errors.Add(ErrorCodes.User_Too_Tall);
        }

        if (string.IsNullOrWhiteSpace(model.UserLocation))
        {
            errors.Add(ErrorCodes.User_Location_Not_Set);
        }

        if (model.UserBudget == null)
        {
            errors.Add(ErrorCodes.User_Budget_Not_Set);
        }
        else
        {
            return Money.Create(model.UserBudget.Value).Switch(
                onSuccess: m => Result<Klant, ErrorCodeSet>.Succeed(new Klant(model.UserHeight!.Value, model.UserLocation!, m)),
                onFailure: e => Result<Klant, ErrorCodeSet>.Fail(errors.Combine(e)));
        }

        return Result<Klant, ErrorCodeSet>.Fail(errors);
    }

    private static Result<FietsType?, ErrorCodeSet> MapFietsType(FietsSearchInputModel model)
    {
        if (model.FietsTypePreference != null)
        {
            return FietsType.Create(model.FietsTypePreference).Switch(
                onSuccess: Result<FietsType?, ErrorCodeSet>.Succeed,
                onFailure: Result<FietsType?, ErrorCodeSet>.Fail);
        }

        return Result<FietsType?, ErrorCodeSet>.Succeed(null);
    }

    public static FietsSearchOutputModel Map(Fiets fiets) =>
        new(fiets.Type.Value, fiets.AantalWielen.Value, fiets.FrameMaat.Value, fiets.Price.Value);
}