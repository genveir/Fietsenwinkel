using Fietsenwinkel.Database.Mappers;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Shopping.Entities;
using Fietsenwinkel.Domain.Shopping.Plugins;
using Fietsenwinkel.Shared.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Fietsen;
internal class FietsenSearch : IFietsInBudgetResolver, IAnyMatchingFietsResolver
{
    public async Task<Result<Fiets, ErrorCodeList>> GetFiets(FiliaalId filiaal, Money budget, FrameMaat min, FrameMaat max, FietsType type) =>
    await GetFietsQuery(filiaal, min, max, budget, type);

    public async Task<Result<Fiets, ErrorCodeList>> GetFiets(FiliaalId filiaal, FrameMaat min, FrameMaat max) =>
        await GetFietsQuery(filiaal, min, max, budget: null, fietsType: null);

    public async Task<Result<Fiets, ErrorCodeList>> GetFiets(FiliaalId filiaal, Money budget, FrameMaat min, FrameMaat max) =>
        await GetFietsQuery(filiaal, min, max, budget, fietsType: null);

    private static async Task<Result<Fiets, ErrorCodeList>> GetFietsQuery(FiliaalId filiaal, FrameMaat min, FrameMaat max,
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
            return Result<Fiets, ErrorCodeList>.Fail([ErrorCodes.No_Matching_Fiets_Found]);
        }

        return FietsMapper.Map(result);
    }
}
