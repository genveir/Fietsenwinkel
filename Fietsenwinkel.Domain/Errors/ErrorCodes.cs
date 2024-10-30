namespace Fietsenwinkel.Domain.Errors;
public enum ErrorCodes
{
    None = 0,

    // Bad Request >= 1000
    Fiets_Has_No_Wheels = 1000,
    Fiets_Has_Too_Many_Wheels = 1002,
    Fietstype_Value_Not_Set = 1001,

    // Internal Server Error >= 10000
    VoorraadNotFound = 10000,
    Fietstype_Invalid_Format = 10001,
    FiliaalId_Value_Not_Set = 10002,
    FiliaalId_Invalid_Format = 10003,
}
