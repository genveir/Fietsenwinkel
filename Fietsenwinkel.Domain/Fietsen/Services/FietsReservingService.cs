using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Fietsen.Services;

public interface IFietsReserver
{
    Task<ErrorResult<ErrorCodeList>> ReserveFietsForUser(Fiets fiets, Klant klant);
}

internal class FietsReservingService : IFietsReserver
{
    public async Task<ErrorResult<ErrorCodeList>> ReserveFietsForUser(Fiets fiets, Klant klant)
    {
        return await Task.FromResult(ErrorResult<ErrorCodeList>.Succeed());

        // niet feitelijk geimplementeerd, maar deze unit zou bijvoorbeeld een fanout kunnen doen naar
        // plugins die messages publishen, email versturen, etc. Taken waar de usecase niet echt om
        // mag caren, want die zegt gewoon "reserveer een fiets", niet welke externe processen daar
        // allemaal dingen mee doen.
    }
}