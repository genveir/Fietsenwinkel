﻿using Fietsenwinkel.Api.Shopping.Models.In;
using Fietsenwinkel.Api.Shopping.Models.Out;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Shopping;

namespace Fietsenwinkel.Api.Shopping.Mappers;

public static class FietsSearchMapper
{
    public static Result<FietsSearchQuery, ErrorCodeList> Map(FietsSearchInputModel model)
    {
        if (model == null)
        {
            return Result<FietsSearchQuery, ErrorCodeList>.Fail([ErrorCodes.No_Input_Model_Provided]);
        }

        return Result.Combine(
            MapKlant(model),
            MapFietsType(model)).Map(
            onSuccess: (klant, fietsType) =>
            {
                var fietsSearchQuery = new FietsSearchQuery(klant, fietsType);

                return Result<FietsSearchQuery, ErrorCodeList>.Succeed(fietsSearchQuery);
            },
            onFailure: Result<FietsSearchQuery, ErrorCodeList>.Fail);
    }

    private static Result<Klant, ErrorCodeList> MapKlant(FietsSearchInputModel model)
    {
        var errors = new ErrorCodeList();

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
            return Money.Create(model.UserBudget.Value).Map(
                onSuccess: m => Result<Klant, ErrorCodeList>.Succeed(new Klant(model.UserHeight!.Value, model.UserLocation!, m)),
                onFailure: e => Result<Klant, ErrorCodeList>.Fail(errors.Combine(e)));
        }

        return Result<Klant, ErrorCodeList>.Fail(errors);
    }

    private static Result<FietsType?, ErrorCodeList> MapFietsType(FietsSearchInputModel model)
    {
        if (model.FietsTypePreference != null)
        {
            return FietsType.Create(model.FietsTypePreference).Map(
                onSuccess: Result<FietsType?, ErrorCodeList>.Succeed,
                onFailure: Result<FietsType?, ErrorCodeList>.Fail);
        }

        return Result<FietsType?, ErrorCodeList>.Succeed(null);
    }

    public static FietsSearchOutputModel Map(FietsAndFiliaalName fietsAndFiliaal) =>
        new(
            fietsAndFiliaal.Fiets.Type.Value,
            fietsAndFiliaal.Fiets.AantalWielen.Value,
            fietsAndFiliaal.Fiets.FrameMaat.Value,
            fietsAndFiliaal.Fiets.Price.Value,
            fietsAndFiliaal.FiliaalName.Value);
}