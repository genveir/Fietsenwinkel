using Fietsenwinkel.Database.Mappers;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Voorraden;

internal class VoorraadRepository : IVoorraadListAccessor
{
    public async Task<Result<VoorraadList, ErrorCodeList>> ListVoorraad(VoorraadListAccessorQuery query)
    {
        using var db = new FietsenwinkelContext();

        var efQuery = db.Voorraden
            .Include(v => v.Fietsen)
            .ThenInclude(f => f.FietsType)
            .Where(v => query.Filiaal.Value == v.FiliaalId);

        if (query.NameFilter != null)
        {
            efQuery = efQuery
                .Where(v => v.Fietsen.Any(f => f.FietsType.TypeName.Contains(query.NameFilter)));
        }

        var result = await efQuery.ToArrayAsync();

        return VoorraadListMapper.Map(result);
    }
}