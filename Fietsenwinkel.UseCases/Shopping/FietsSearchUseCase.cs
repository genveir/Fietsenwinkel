using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Domain.Shopping.Services;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Shopping.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Shopping;

public record FietsSearchQuery(Klant Klant, FietsType? PreferredType);

public interface IFietsSearchUseCase
{
    Task<Result<Fiets, ErrorCodeList>> Search(FietsSearchQuery query);
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

    public async Task<Result<Fiets, ErrorCodeList>> Search(FietsSearchQuery query)
    {
        var bestFietsQuery = new DetermineBestFietsForKlantQuery(query.Klant, query.PreferredType);

        var bestFietsResult = await determineBestFietsForKlantService.DetermineBestFiets(bestFietsQuery);

        return await bestFietsResult.Switch(
            onSuccess: ReserveFiets,
            onFailure: errors => Task.FromResult(Result<Fiets, ErrorCodeList>.Fail(errors)));

        async Task<Result<Fiets, ErrorCodeList>> ReserveFiets(Fiets bestFiets)
        {
            var reserveResult = await fietsReserver.ReserveFietsForUser(bestFiets, query.Klant);

            // om de een of andere reden moeten we de fiets opnieuw fetchen na het reserveren
            return await reserveResult.Switch(
                onSuccess: () => fietsRefetcher.RefetchFiets(bestFiets),
                onFailure: errors => Task.FromResult(Result<Fiets, ErrorCodeList>.Fail(errors)));
        }
    }
}