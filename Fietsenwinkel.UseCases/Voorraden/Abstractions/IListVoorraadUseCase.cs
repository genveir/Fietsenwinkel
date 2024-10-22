using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden.Abstractions;
public interface IListVoorraadUseCase
{
    Task<Result<Voorraad, ErrorCodeSet>> GetVoorraad(ListVoorraadQuery query);
}
