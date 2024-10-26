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
        ErrorCodeSet errors = [];

        if (model == null)
        {
            return Result<FietsSearchQuery, ErrorCodeSet>.Fail([ErrorCodes.No_Input_Model_Provided]);
        }

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

        Money budget = Money.Default();
        if (model.UserBudget == null)
        {
            errors.Add(ErrorCodes.User_Budget_Not_Set);
        }
        else
        {
            Money.Create(model.UserBudget.Value).Switch(
                onSuccess: m => budget = m,
                onFailure: errors.AddRange);
        }

        FietsType? fietsTypePreference = null;
        if (model.FietsTypePreference != null)
        {
            FietsType.Create(model.FietsTypePreference).Switch(
                onSuccess: ft => fietsTypePreference = ft,
                onFailure: errors.AddRange);
        }

        if (errors.Count > 0)
        {
            return Result<FietsSearchQuery, ErrorCodeSet>.Fail(errors);
        }

        var klant = new Klant(model.UserHeight!.Value, model.UserLocation!, budget);

        return Result<FietsSearchQuery, ErrorCodeSet>.Succeed(new(klant, fietsTypePreference));
    }

    public static FietsSearchOutputModel Map(Fiets fiets) =>
        new(fiets.Type.Value, fiets.AantalWielen.Value, fiets.FrameMaat.Value, fiets.Price.Value);
}