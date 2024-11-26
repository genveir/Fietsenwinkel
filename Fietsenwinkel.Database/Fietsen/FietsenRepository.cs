using Fietsenwinkel.Database.Mappers;
using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Fietsen.Plugins;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Shopping.Plugins;
using Fietsenwinkel.UseCases.Voorraden.Management.Plugins;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Fietsen;

internal sealed class FietsenRepository : IDisposable, IVoorraadDetailsAccessor, IFietsRefetcher, IFietsAccessor, IFietsUpdater, IFietsDeleter
{
    private readonly FietsenwinkelContext db;

    public FietsenRepository(FietsenwinkelContext db)
    {
        this.db = db;
    }

    public async Task<Result<VoorraadDetails, ErrorCodeList>> GetVoorraadDetails(VoorraadDetailsAccessorQuery query)
    {
        var result = await db.Fietsen
            .Include(f => f.FietsType)
            .Where(f => f.Voorraad.FiliaalId == query.FiliaalId.Value)
            .OrderBy(f => f.Id)
            .ToArrayAsync();

        return VoorraadDetailsMapper.Map(query.FiliaalId, result);
    }

    public async Task<ErrorResult<ErrorCodeList>> FietsExists(FietsId fietsId)
    {
        var exists = await db.Fietsen.AnyAsync(f => f.Id == fietsId.Value);

        return exists
            ? ErrorResult<ErrorCodeList>.Succeed()
            : ErrorResult<ErrorCodeList>.Fail([ErrorCodes.Fiets_Not_Found]);
    }

    public async Task<ErrorResult<ErrorCodeList>> FietsExistsAtFiliaal(FietsId fietsId, FiliaalId filiaalId)
    {
        var filiaalExists = await db.Filialen.AnyAsync(f => f.Id == filiaalId.Value);

        return await (filiaalExists
            ? CheckForFiets()
            : ErrorResult<ErrorCodeList>.FailAsTask([ErrorCodes.Filiaal_Not_Found]));

        async Task<ErrorResult<ErrorCodeList>> CheckForFiets()
        {
            var fietsExistsAtFiliaal = await db.Fietsen
                .AnyAsync(f => f.Voorraad.FiliaalId == filiaalId.Value && f.Id == fietsId.Value);

            return fietsExistsAtFiliaal
                ? ErrorResult<ErrorCodeList>.Succeed()
                : ErrorResult<ErrorCodeList>.Fail([ErrorCodes.Fiets_Not_Found]);
        }
    }

    internal async Task<FietsModel?> GetFietsById(int fietsId)
    {
        return await db.Fietsen
            .Include(f => f.FietsType)
            .SingleOrDefaultAsync(f => f.Id == fietsId);
    }

    public async Task<FietsModel?> GetFietsById(FietsId fietsId) =>
        await GetFietsById(fietsId.Value);

    public async Task<Result<Fiets, ErrorCodeList>> GetFiets(FietsId fietsId)
    {
        var model = await GetFietsById(fietsId);

        if (model == null)
        {
            return Result<Fiets, ErrorCodeList>.Fail([ErrorCodes.Fiets_Not_Found]);
        }

        return FietsMapper.Map(model);
    }

    // kijk, deze had ik liever bij de voorraden gehad, maar kan niet want ik heb de interface niet gesplitst
    public async Task<Result<Fiets, ErrorCodeList>> GetFietsAtFiliaal(FietsId fietsId, FiliaalId filiaalId)
    {
        var model = await db.Fietsen
            .Include(f => f.FietsType)
            .Where(f => f.Voorraad.FiliaalId == filiaalId.Value)
            .SingleOrDefaultAsync(f => f.Id == fietsId.Value);

        if (model == null)
        {
            return Result<Fiets, ErrorCodeList>.Fail([ErrorCodes.Fiets_Not_Found]);
        }

        return FietsMapper.Map(model);
    }

    internal async Task<Result<FietsTypeModel, ErrorCodeList>> GetFietsType(FietsType fietsType)
    {
        var result = await db.FietsTypes
            .FirstOrDefaultAsync(f => f.TypeName == fietsType.Value);

        if (result == null)
        {
            return Result<FietsTypeModel, ErrorCodeList>.Fail([ErrorCodes.FietsType_Not_Found]);
        }

        return Result<FietsTypeModel, ErrorCodeList>.Succeed(result);
    }

    public async Task<Result<Fiets, ErrorCodeList>> RefetchFiets(Fiets fiets)
    {
        var result = await GetFietsById(fiets.Id);

        if (result == null)
        {
            return Result<Fiets, ErrorCodeList>.Fail([ErrorCodes.Fiets_Cannot_Be_Refetched]);
        }

        return FietsMapper.Map(result);
    }

    public async Task<Result<Fiets, ErrorCodeList>> Update(FietsUpdateModel fietsUpdateModel)
    {
        return await Result.Combine(
            await GetFietsType(fietsUpdateModel.FietsType),
            await GetFietsForUpdate(fietsUpdateModel.Id))
            .Map(
                onSuccess: UpdateFietsFields,
                onFailure: Result<Fiets, ErrorCodeList>.FailAsTask);

        async Task<Result<Fiets, ErrorCodeList>> UpdateFietsFields(FietsTypeModel fietsType, FietsModel fiets)
        {
            fiets.FietsTypeId = fietsType.Id;
            fiets.Price = fietsUpdateModel.Price.Value;
            fiets.FrameMaat = fietsUpdateModel.FrameMaat.Value;
            fiets.AantalWielen = fietsUpdateModel.AantalWielen.Value;

            await db.SaveChangesAsync();

            var queriedForOutput = await GetFietsById(fiets.Id);

            return FietsMapper.Map(queriedForOutput ?? throw new InvalidDataException("hee de fiets is weg direct na de update, dit is niet goed"));
        }
    }

    private async Task<Result<FietsModel, ErrorCodeList>> GetFietsForUpdate(FietsId fietsId)
    {
        var model = await db.Fietsen
            .SingleOrDefaultAsync(f => f.Id == fietsId.Value);

        if (model == null)
        {
            return Result<FietsModel, ErrorCodeList>.Fail([ErrorCodes.Fiets_Not_Found]);
        }
        return Result<FietsModel, ErrorCodeList>.Succeed(model);
    }

    public async Task<ErrorResult<ErrorCodeList>> DeleteFiets(FietsId id)
    {
        var model = await db.Fietsen
            .SingleOrDefaultAsync(f => f.Id == id.Value);

        if (model == null)
        {
            return ErrorResult<ErrorCodeList>.Fail([ErrorCodes.Fiets_Not_Found]);
        }

        db.Fietsen.Remove(model);
        await db.SaveChangesAsync();

        return ErrorResult<ErrorCodeList>.Succeed();
    }

    public void Dispose()
    {
        db.Dispose();
    }
}