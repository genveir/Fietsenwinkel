using Fietsenwinkel.Database.Mappers;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Fietsen;

internal class FietsenRepository : IVoorraadDetailsAccessor
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
}