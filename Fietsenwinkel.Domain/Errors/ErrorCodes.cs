namespace Fietsenwinkel.Domain.Errors;
public enum ErrorCodes
{
    None = 0,

    // Bad Request >= 1000
    Fiets_Has_No_Wheels = 1000,

    // Internal Server Error >= 10000
    VoorraadNotFound = 10000
}
