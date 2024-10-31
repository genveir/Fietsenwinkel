using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Domain.Filialen.Entities;
using Fietsenwinkel.Domain.Shopping.Plugins;
using Fietsenwinkel.Shared.Results;
using MapsApiClient;

namespace Fietsenwinkel.MapsApiAdapter;

public class MapsApiLocationResolver : IDistanceResolver
{
    private readonly IMapsApiClient mapsApiClient;

    public MapsApiLocationResolver(IMapsApiClient mapsApiClient)
    {
        this.mapsApiClient = mapsApiClient;
    }

    // als je hier dingen wil doen waarvoor je de MapCoordinates nog nodig hebt heb je weinig keuze behalve hier
    // een cache bijhouden van domain coordinates en api coordinates. Als object op de domain positions gooien
    // kan ook, maar dan moet je ze alsnog hier weer uitlezen of naar dynamic casten om ze te kunnen gebruiken.

    public async Task<Result<int, ErrorCodeList>> ResolveDistanceBetween(string location, FiliaalName filiaalName)
    {
        try
        {
            var coordinate = await mapsApiClient.ResolvePosition(thingToLocate: location);
            var shopLocation = await mapsApiClient.ResolvePosition(thingToLocate: filiaalName.Value);

            var distance = coordinate.GetDistanceTo(shopLocation);

            return Result<int, ErrorCodeList>.Succeed(distance);
        }
        catch (KanIkNietVindenException)
        {
            return Result<int, ErrorCodeList>.Fail([ErrorCodes.MapsApi_Cannot_Locate_Place]);
        }
        catch (Exception)
        {
            return Result<int, ErrorCodeList>.Fail([ErrorCodes.MapsApi_Cannot_Be_Reached]);
        }
    }
}