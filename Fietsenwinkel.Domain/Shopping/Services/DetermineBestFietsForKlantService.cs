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
    Task<Result<Fiets, ErrorCodeSet>> DetermineBestFiets(DetermineBestFietsForKlantQuery query);
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

    public async Task<Result<Fiets, ErrorCodeSet>> DetermineBestFiets(DetermineBestFietsForKlantQuery query)
    {
        var (min, max) = FrameMaatService.DetermineSizesFor(query.Klant.Height);

        var filiaalListResult = await filiaalListAccessor.ListFilialen();

        return await filiaalListResult.Switch(
            onSuccess: DetermineClosest,
            onFailure: errors => Task.FromResult(Result<Fiets, ErrorCodeSet>.Fail(errors)));

        async Task<Result<Fiets, ErrorCodeSet>> DetermineClosest(FiliaalList filiaalList)
        {
            var closestResult = await DetermineNearestFiliaal(filiaalList, query.Klant.Location);

            return await closestResult.Switch(
                onSuccess: DetermineInBudget,
                onFailure: errors => Task.FromResult(Result<Fiets, ErrorCodeSet>.Fail(errors)));
        }

        async Task<Result<Fiets, ErrorCodeSet>> DetermineInBudget(FiliaalId filiaal)
        {
            var budgetResult = await this.DetermineInBudget(filiaal, min, max, query);

            return await budgetResult.Switch(
                onSuccess: fiets => Task.FromResult(Result<Fiets, ErrorCodeSet>.Succeed(fiets)),
                onFailure: errors =>
                {
                    if (errors.Count == 1 && errors.Single() == ErrorCodes.No_Matching_Fiets_Found)
                    {
                        return DetermineAny(filiaal);
                    }
                    else return Task.FromResult(Result<Fiets, ErrorCodeSet>.Fail(errors));
                });
        }

        async Task<Result<Fiets, ErrorCodeSet>> DetermineAny(FiliaalId filiaal)
        {
            return await anyMatchingFietsResolver.GetFiets(filiaal, min, max);
        }
    }

    private async Task<Result<Fiets, ErrorCodeSet>> DetermineInBudget(FiliaalId filiaal, FrameMaat min, FrameMaat max, DetermineBestFietsForKlantQuery query)
    {
        if (query.PreferredFietsType != null)
        {
            var preferredResult = await fietsInBudgetResolver.GetFiets(filiaal, query.Klant.Budget, min, max, query.PreferredFietsType);

            return await preferredResult.Switch(
                onSuccess: f => Task.FromResult(Result<Fiets, ErrorCodeSet>.Succeed(f)),
                onFailure: errors =>
                {
                    if (errors.Count == 1 && errors.Single() == ErrorCodes.No_Matching_Fiets_Found)
                    {
                        return DetermineWithoutPreference(filiaal, query);
                    }
                    return Task.FromResult(Result<Fiets, ErrorCodeSet>.Fail(errors));
                });
        }

        return await DetermineWithoutPreference(filiaal, query);

        async Task<Result<Fiets, ErrorCodeSet>> DetermineWithoutPreference(FiliaalId filiaal, DetermineBestFietsForKlantQuery query)
        {
            return await fietsInBudgetResolver.GetFiets(filiaal, query.Klant.Budget, min, max);
        }
    }

    private async Task<Result<FiliaalId, ErrorCodeSet>> DetermineNearestFiliaal(FiliaalList filiaalList, string klantLocation)
    {
        List<(FiliaalId filiaal, int distance)> distances = [];

        ErrorCodeSet errors = [];

        foreach (var filiaal in filiaalList.FiliaalListEntries)
        {
            var distanceResult = await distanceResolver.ResolveDistanceBetween(klantLocation, filiaal.Name);

            distanceResult.Switch(
                onSuccess: d => distances.Add((filiaal.Id, d)),
                onFailure: e =>
                {
                    if (e.Count == 1 && e.Single() == ErrorCodes.MapsApi_Cannot_Locate_Place)
                    {
                        distances.Add((filiaal.Id, int.MaxValue));
                    }
                    else
                    {
                        errors = e;
                    }
                });

            if (errors.Count > 0)
            {
                return Result<FiliaalId, ErrorCodeSet>.Fail(errors);
            }
        }

        var closest = distances
            .OrderByDescending(d => d.distance)
            .FirstOrDefault();

        return Result<FiliaalId, ErrorCodeSet>.Succeed(closest.filiaal);
    }
}