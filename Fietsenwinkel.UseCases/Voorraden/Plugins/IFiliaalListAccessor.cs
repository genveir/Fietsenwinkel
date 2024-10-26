using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden.Plugins;

public interface IFiliaalListAccessor
{
    Task<Result<FiliaalList, ErrorCodeSet>> ListFilialen();
}