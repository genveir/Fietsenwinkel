using Fietsenwinkel.Database.Mappers;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Domain.Shopping.Plugins;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Shopping.Plugins;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Fietsen;

internal class FietsenRepository : IVoorraadDetailsAccessor, IFietsRefetcher, IFietsInBudgetResolver, IAnyMatchingFietsResolver
{
    public async Task<Result<VoorraadDetails, ErrorCodeSet>> GetVoorraadDetails(VoorraadDetailsAccessorQuery query)
    {
        using var db = new FietsenwinkelContext();

        var result = await db.Fietsen
            .Include(f => f.FietsType)
            .Where(f => f.Voorraad.FiliaalId == query.FiliaalId.Value)
            .ToArrayAsync();

        return VoorraadDetailsMapper.Map(query.FiliaalId, result);
    }

    public async Task<Result<Fiets, ErrorCodeSet>> RefetchFiets(Fiets fiets)
    {
        using var db = new FietsenwinkelContext();

        // oh ja.. ik heb geen Id. Also is het 21:14 op zaterdag dus dan maar zo.
        var result = await db.Fietsen
            .Include(f => f.FietsType)
            .Where(f => f.FietsType.TypeName == fiets.Type.Value)
            .Where(f => f.AantalWielen == fiets.AantalWielen.Value)
            .Where(f => f.Price == fiets.Price.Value)
            .Where(f => f.FrameMaat == fiets.FrameMaat.Value)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return Result<Fiets, ErrorCodeSet>.Fail([ErrorCodes.Fiets_Cannot_Be_Refetched]);
        }

        return FietsMapper.Map(result);
    }

    public async Task<Result<Fiets, ErrorCodeSet>> GetFiets(FiliaalId filiaal, Money budget, FrameMaat min, FrameMaat max, FietsType type) =>
        await GetFietsQuery(filiaal, min, max, budget, type);

    public async Task<Result<Fiets, ErrorCodeSet>> GetFiets(FiliaalId filiaal, FrameMaat min, FrameMaat max) =>
        await GetFietsQuery(filiaal, min, max, budget: null, fietsType: null);

    public async Task<Result<Fiets, ErrorCodeSet>> GetFiets(FiliaalId filiaal, Money budget, FrameMaat min, FrameMaat max) =>
        await GetFietsQuery(filiaal, min, max, budget, fietsType: null);

    private static async Task<Result<Fiets, ErrorCodeSet>> GetFietsQuery(FiliaalId filiaal, FrameMaat min, FrameMaat max,
        Money? budget, FietsType? fietsType)
    {
        using var db = new FietsenwinkelContext();

        var query = db.Fietsen
            .Include(f => f.FietsType)
            .Where(f => f.FrameMaat >= min.Value)
            .Where(f => f.FrameMaat <= max.Value)
            .Where(f => f.Voorraad.FiliaalId == filiaal.Value);

        if (budget != null)
        {
            query = query.Where(f => f.Price <= budget.Value);
        }

        if (fietsType != null)
        {
            query = query.Where(f => f.FietsType.TypeName == fietsType.Value);
        }

        var result = await query
            .OrderByDescending(f => f.Price)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return Result<Fiets, ErrorCodeSet>.Fail([ErrorCodes.No_Matching_Fiets_Found]);
        }

        return FietsMapper.Map(result);
    }
}