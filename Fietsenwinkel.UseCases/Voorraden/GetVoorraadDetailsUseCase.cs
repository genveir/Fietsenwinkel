using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Filialen.Plugins;
using Fietsenwinkel.Domain.Voorraden.Entities;
using Fietsenwinkel.Shared.Results;
using Fietsenwinkel.UseCases.Voorraden.Plugins;
using System.Threading.Tasks;

namespace Fietsenwinkel.UseCases.Voorraden;

public interface IGetVoorraadDetailsUseCase
{
    Task<Result<VoorraadDetails, ErrorCodeList>> GetDetails(FiliaalId filiaalId);
}

internal class GetVoorraadDetailsUseCase : IGetVoorraadDetailsUseCase
{
    private readonly IVoorraadDetailsAccessor voorraadDetailsAccessor;
    private readonly IFiliaalExistenceChecker filiaalExistenceChecker;

    public GetVoorraadDetailsUseCase(IVoorraadDetailsAccessor voorraadDetailsAccessor, IFiliaalExistenceChecker filiaalExistenceChecker)
    {
        this.voorraadDetailsAccessor = voorraadDetailsAccessor;
        this.filiaalExistenceChecker = filiaalExistenceChecker;
    }

    public async Task<Result<VoorraadDetails, ErrorCodeList>> GetDetails(FiliaalId filiaalId)
    {
        var filiaalExistsResult = await filiaalExistenceChecker.Exists(filiaalId);

        return await filiaalExistsResult
            .Map(
                onFailure: Result<VoorraadDetails, ErrorCodeList>.FailAsTask,
                onSuccess: GetVoorraadDetails
            );

        async Task<Result<VoorraadDetails, ErrorCodeList>> GetVoorraadDetails()
        {
            var query = new VoorraadDetailsAccessorQuery(filiaalId);

            return await voorraadDetailsAccessor.GetVoorraadDetails(query);
        }
    }
}