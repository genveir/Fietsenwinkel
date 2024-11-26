using Fietsenwinkel.Database.Mappers;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Filialen.Plugins;
using Fietsenwinkel.Domain.Shopping.Plugins;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Filialen;

internal class FilialenRepository : IFiliaalListAccessor, IFiliaalExistenceChecker, IShoppingFiliaalListAccessor
{
    public async Task<Result<FiliaalList, ErrorCodeList>> ListFilialen()
    {
        using var db = new FietsenwinkelContext();

        var filialen = await db.Filialen.ToArrayAsync();

        return FiliaalListMapper.Map(filialen);
    }

    public async Task<ErrorResult<ErrorCodeList>> Exists(FiliaalId filiaalId)
    {
        using var db = new FietsenwinkelContext();

        var exists = await db.Filialen.AnyAsync(f => f.Id == filiaalId.Value);

        return exists
            ? ErrorResult<ErrorCodeList>.Succeed()
            : ErrorResult<ErrorCodeList>.Fail([ErrorCodes.Filiaal_Not_Found]);
    }
}