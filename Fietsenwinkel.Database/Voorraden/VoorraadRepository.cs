using Fietsenwinkel.Database.Fietsen;
using Fietsenwinkel.Database.Mappers;
using Fietsenwinkel.Database.Models;
using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Fietsen.Entities;
using Fietsenwinkel.Domain.Fietsen.Plugins;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Management;
using Fietsenwinkel.UseCases.Voorraden.Management.Plugins;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fietsenwinkel.Database.Voorraden;

internal class VoorraadRepository : IVoorraadListAccessor, IFietsToVoorraadAdder, IFietsMover
{
    private readonly FietsenwinkelContext db;

    public VoorraadRepository(FietsenwinkelContext db)
    {
        this.db = db;
    }

    public async Task<Result<FietsId, ErrorCodeList>> AddFietsToVoorraad(FietsCreateModel fietsCreateModel)
    {
        return await Result.Combine(
            await GetVoorraadForFiliaal(fietsCreateModel.Filiaal),
            await new FietsenRepository(db).GetFietsType(fietsCreateModel.Type))
                .Map(
                    onSuccess: async (voorraad, fietsType) =>
                    {
                        var fiets = new FietsModel
                        {
                            FietsTypeId = fietsType.Id,
                            AantalWielen = fietsCreateModel.AantalWielen.Value,
                            FrameMaat = fietsCreateModel.FrameMaat.Value,
                            Price = fietsCreateModel.Price.Value,
                            VoorraadId = voorraad.Id
                        };

                        db.Fietsen.Add(fiets);

                        voorraad.Fietsen.Add(fiets);
                        await db.SaveChangesAsync();

                        return FietsId.Create(fiets.Id)
                            .Map(
                                onSuccess: id => Result<FietsId, ErrorCodeList>.Succeed(id),
                                onFailure: Result<FietsId, ErrorCodeList>.Fail);
                    },
                    onFailure: Result<FietsId, ErrorCodeList>.FailAsTask);
    }

    private async Task<Result<VoorraadModel, ErrorCodeList>> GetVoorraadForFiliaal(FiliaalId filiaalId)
    {
        var voorraad = await db.Voorraden
            .FirstOrDefaultAsync(v => v.FiliaalId == filiaalId.Value);

        if (voorraad == null)
        {
            return Result<VoorraadModel, ErrorCodeList>.Fail([ErrorCodes.Filiaal_Not_Found]);
        }

        return Result<VoorraadModel, ErrorCodeList>.Succeed(voorraad);
    }


    public async Task<Result<VoorraadList, ErrorCodeList>> ListVoorraad(VoorraadListAccessorQuery query)
    {
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

    public async Task<ErrorResult<ErrorCodeList>> MoveFietsToVoorraadOfFiliaal(FietsId fiets, FiliaalId filiaalId)
    {
        var voorraad = await db.Voorraden
            .FirstOrDefaultAsync(v => v.FiliaalId == filiaalId.Value);

        ErrorCodeList errors = [];
        if (voorraad == null)
        {
            errors.Add(ErrorCodes.Filiaal_Not_Found);
        }

        var fietsModel = await db.Fietsen
            .FirstOrDefaultAsync(f => f.Id == fiets.Value);

        if (fietsModel == null)
        {
            errors.Add(ErrorCodes.Fiets_Not_Found);
        }

        if (errors.Count != 0)
        {
            return ErrorResult<ErrorCodeList>.Fail(errors);
        }

        return await MoveFietsToVoorraad(fietsModel!, voorraad!);
    }

    private async Task<ErrorResult<ErrorCodeList>> MoveFietsToVoorraad(FietsModel fiets, VoorraadModel voorraad)
    {
        fiets.VoorraadId = voorraad.Id;
        await db.SaveChangesAsync();

        return ErrorResult<ErrorCodeList>.Succeed();
    }
}