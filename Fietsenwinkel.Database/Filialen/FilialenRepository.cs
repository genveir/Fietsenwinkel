using Fietsenwinkel.Database.Mappers;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Filialen;

internal class FilialenRepository : IFiliaalListAccessor
{
    public async Task<Result<FiliaalList, ErrorCodeSet>> ListFilialen()
    {
        using var db = new FietsenwinkelContext();

        var filialen = await db.Filialen.ToArrayAsync();

        return FiliaalListMapper.Map(filialen);
    }
}