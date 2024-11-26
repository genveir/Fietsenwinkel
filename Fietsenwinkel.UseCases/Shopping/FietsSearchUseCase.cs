using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Fietsen.Services;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Domain.Shopping.Services;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Shopping.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Shopping;

public record FietsSearchQuery(Klant Klant, FietsType? PreferredType);

public interface IFietsSearchUseCase
{
    Task<Result<FietsAndFiliaalName, ErrorCodeList>> Search(FietsSearchQuery query);
}

internal class FietsSearchUseCase : IFietsSearchUseCase
{
    private readonly IDetermineBestFietsForKlantService determineBestFietsForKlantService;
    private readonly IFietsReserver fietsReserver;
    private readonly IFietsRefetcher fietsRefetcher;

    public FietsSearchUseCase(IDetermineBestFietsForKlantService determineBestFietsForKlantService, IFietsReserver fietsReserver, IFietsRefetcher fietsRefetcher)
    {
        this.determineBestFietsForKlantService = determineBestFietsForKlantService;
        this.fietsReserver = fietsReserver;
        this.fietsRefetcher = fietsRefetcher;
    }

    public async Task<Result<FietsAndFiliaalName, ErrorCodeList>> Search(FietsSearchQuery query)
    {
        var bestFietsQuery = new DetermineBestFietsForKlantQuery(query.Klant, query.PreferredType);

        var bestFietsResult = await determineBestFietsForKlantService.DetermineBestFiets(bestFietsQuery);

        return await bestFietsResult.Map(
            onSuccess: ReserveFiets,
            onFailure: Result<FietsAndFiliaalName, ErrorCodeList>.FailAsTask);

        async Task<Result<FietsAndFiliaalName, ErrorCodeList>> ReserveFiets(FietsAndFiliaalName bestFiets)
        {
            var reserveResult = await fietsReserver.ReserveFietsForUser(bestFiets.Fiets, query.Klant);

            // om de een of andere reden moeten we de fiets opnieuw fetchen na het reserveren
            return await reserveResult.Map(
                onSuccess: () => RefetchFiets(bestFiets),
                onFailure: Result<FietsAndFiliaalName, ErrorCodeList>.FailAsTask);
        }

        async Task<Result<FietsAndFiliaalName, ErrorCodeList>> RefetchFiets(FietsAndFiliaalName bestFiets)
        {
            var refetchResult = await fietsRefetcher.RefetchFiets(bestFiets.Fiets);

            return refetchResult.Map(
                onSuccess: f => Result<FietsAndFiliaalName, ErrorCodeList>.Succeed(new FietsAndFiliaalName(f, bestFiets.FiliaalName)),
                onFailure: Result<FietsAndFiliaalName, ErrorCodeList>.Fail);
        }
    }
}