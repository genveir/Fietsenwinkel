using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Shared.Results;
using System.Threading.Tasks;

namespace Fietsenwinkel.Domain.Fietsen.Plugins;

public record FietsUpdateModel(FietsId Id, FietsType FietsType, AantalWielen AantalWielen, FrameMaat FrameMaat, Money Price);

public interface IFietsUpdater
{
    Task<Result<Fiets, ErrorCodeList>> Update(FietsUpdateModel fietsUpdateModel);
}
