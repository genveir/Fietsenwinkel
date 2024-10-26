namespace Fietsenwinkel.Domain.Errors;

public enum ErrorCodes
{
    None = 0,

    // Not Found

    Voorraad_Not_Found = 1,
    Filiaal_Not_Found = 2,

    // Bad Request >= 1000

    Fiets_Has_No_Wheels = 1000,
    Fiets_Has_Too_Many_Wheels = 1001,
    Fietstype_Value_Not_Set = 1002,
    Fietstype_Invalid_Format = 1003,
    FiliaalId_Value_Not_Set = 1004,
    FiliaalId_Invalid_Format = 1005,
    FiliaalName_Value_NotSet = 1006,
    FiliaalName_Invalid_Format = 1007,
    AantalWielen_Value_Not_Set = 1008,

    // Internal Server Error >= 10000

    Legacy_Voorraden_Have_Different_FiliaalIds = 10000,
    Fiets_FrameMaat_Invalid = 10001,
}