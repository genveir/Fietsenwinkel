using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Shopping.Plugins;

namespace Fietsenwinkel.Application.Reservations;

internal class FietsReserver : IFietsReserver
{
    public async Task<ErrorResult<ErrorCodeSet>> ReserveFietsForUser(Fiets fiets, Klant klant)
    {
        return await Task.FromResult(ErrorResult<ErrorCodeSet>.Succeed());

        // niet feitelijk geimplementeerd, maar deze unit zou bijvoorbeeld een fanout kunnen doen naar
        // plugins die messages publishen, email versturen, etc. Taken waar de usecase niet echt om
        // mag caren, want die zegt gewoon "reserveer een fiets", niet welke externe processen daar
        // allemaal dingen mee doen.
    }
}