using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Domain.Shopping.Plugins;
using Fietsenwinkel.Shared.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Shopping.Services;

public record DetermineBestFietsForKlantQuery(Klant Klant, FietsType? PreferredFietsType);

public interface IDetermineBestFietsForKlantService
{
    Task<Result<FietsAndFiliaalName, ErrorCodeList>> DetermineBestFiets(DetermineBestFietsForKlantQuery query);
}

internal class DetermineBestFietsForKlantService : IDetermineBestFietsForKlantService
{
    private readonly IShoppingFiliaalListAccessor filiaalListAccessor;
    private readonly IDistanceResolver distanceResolver;
    private readonly IFietsInBudgetResolver fietsInBudgetResolver;
    private readonly IAnyMatchingFietsResolver anyMatchingFietsResolver;

    public DetermineBestFietsForKlantService(IShoppingFiliaalListAccessor filiaalListAccessor, IDistanceResolver distanceResolver,
        IFietsInBudgetResolver fietsInBudgetResolver, IAnyMatchingFietsResolver anyMatchingFietsResolver)
    {
        this.filiaalListAccessor = filiaalListAccessor;
        this.distanceResolver = distanceResolver;
        this.fietsInBudgetResolver = fietsInBudgetResolver;
        this.anyMatchingFietsResolver = anyMatchingFietsResolver;
    }

    public async Task<Result<FietsAndFiliaalName, ErrorCodeList>> DetermineBestFiets(DetermineBestFietsForKlantQuery query)
    {
        var frameMaatResult = FrameMaatService.DetermineSizesFor(query.Klant.Height);

        var filiaalListResult = await filiaalListAccessor.ListFilialen();

        return await filiaalListResult.Map(
            onSuccess: DetermineClosest,
            onFailure: Result<FietsAndFiliaalName, ErrorCodeList>.FailAsTask);

        async Task<Result<FietsAndFiliaalName, ErrorCodeList>> DetermineClosest(FiliaalList filiaalList)
        {
            var closestResult = await DetermineNearestFiliaal(filiaalList, query.Klant.Location);

            return await closestResult.Map(
                onSuccess: DetermineInBudget,
                onFailure: Result<FietsAndFiliaalName, ErrorCodeList>.FailAsTask);
        }

        async Task<Result<FietsAndFiliaalName, ErrorCodeList>> DetermineInBudget(FiliaalListEntry filiaal)
        {
            var budgetResult = await frameMaatResult.Map(
                onSuccess: sizes => this.DetermineInBudget(filiaal, sizes.min, sizes.max, query),
                onFailure: Result<FietsAndFiliaalName, ErrorCodeList>.FailAsTask);

            return await budgetResult.Map(
                onSuccess: fietsAndFiliaal => Task.FromResult(Result<FietsAndFiliaalName, ErrorCodeList>.Succeed(fietsAndFiliaal)),
                onFailure: errors =>
                {
                    if (errors.Count == 1 && errors.Single() == ErrorCodes.No_Matching_Fiets_Found)
                    {
                        return DetermineAny(filiaal);
                    }
                    else return Result<FietsAndFiliaalName, ErrorCodeList>.FailAsTask(errors);
                });
        }

        async Task<Result<FietsAndFiliaalName, ErrorCodeList>> DetermineAny(FiliaalListEntry filiaal)
        {
            var anyResult = await frameMaatResult.Map(
                onSuccess: sizes => anyMatchingFietsResolver.GetFiets(filiaal.Id, sizes.min, sizes.max),
                onFailure: Result<Fiets, ErrorCodeList>.FailAsTask);

            return anyResult.Map(
                onSuccess: f => Result<FietsAndFiliaalName, ErrorCodeList>.Succeed(new(f, filiaal.Name)),
                onFailure: Result<FietsAndFiliaalName, ErrorCodeList>.Fail);
        }
    }

    private async Task<Result<FietsAndFiliaalName, ErrorCodeList>> DetermineInBudget(FiliaalListEntry filiaal, FrameMaat min, FrameMaat max, DetermineBestFietsForKlantQuery query)
    {
        if (query.PreferredFietsType != null)
        {
            var preferredResult = await fietsInBudgetResolver.GetFiets(filiaal.Id, query.Klant.Budget, min, max, query.PreferredFietsType);

            return await preferredResult.Map(
                onSuccess: f => Task.FromResult(Result<FietsAndFiliaalName, ErrorCodeList>.Succeed(new(f, filiaal.Name))),
                onFailure: errors =>
                {
                    if (errors.Count == 1 && errors.Single() == ErrorCodes.No_Matching_Fiets_Found)
                    {
                        return DetermineWithoutPreference(filiaal, query);
                    }
                    return Result<FietsAndFiliaalName, ErrorCodeList>.FailAsTask(errors);
                });
        }

        return await DetermineWithoutPreference(filiaal, query);

        async Task<Result<FietsAndFiliaalName, ErrorCodeList>> DetermineWithoutPreference(FiliaalListEntry filiaal, DetermineBestFietsForKlantQuery query)
        {
            var withoutPreferenceResult = await fietsInBudgetResolver.GetFiets(filiaal.Id, query.Klant.Budget, min, max);

            return withoutPreferenceResult.Map(
                onSuccess: f => Result<FietsAndFiliaalName, ErrorCodeList>.Succeed(new(f, filiaal.Name)),
                onFailure: Result<FietsAndFiliaalName, ErrorCodeList>.Fail);
        }
    }

    private async Task<Result<FiliaalListEntry, ErrorCodeList>> DetermineNearestFiliaal(FiliaalList filiaalList, string klantLocation)
    {
        List<(FiliaalListEntry filiaal, int distance)> distances = [];

        ErrorCodeList errors = [];

        foreach (var filiaal in filiaalList.FiliaalListEntries)
        {
            var distanceResult = await distanceResolver.ResolveDistanceBetween(klantLocation, filiaal.Name);

            distanceResult.Act(
                onSuccess: d => distances.Add((filiaal, d)),
                onFailure: e =>
                {
                    if (e.Count == 1 && e.Single() == ErrorCodes.MapsApi_Cannot_Locate_Place)
                    {
                        distances.Add((filiaal, int.MaxValue));
                    }
                    else
                    {
                        errors = e;
                    }
                });

            if (errors.Count > 0)
            {
                return Result<FiliaalListEntry, ErrorCodeList>.Fail(errors);
            }
        }

        var closest = distances
            .OrderByDescending(d => d.distance)
            .FirstOrDefault();

        return Result<FiliaalListEntry, ErrorCodeList>.Succeed(closest.filiaal);
    }
}