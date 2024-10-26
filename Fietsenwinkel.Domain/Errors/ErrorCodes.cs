namespace Fietsenwinkel.Domain.Errors;

public enum ErrorCodes
{
    None = 0,

    // Not Found

    Voorraad_Not_Found = 1,
    Filiaal_Not_Found = 2,
    No_Matching_Fiets_Found = 3,

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
    Fiets_FrameMaat_Invalid = 1009,
    Money_Value_Not_Set = 1010,
    User_Too_Short = 1011,
    User_Too_Tall = 1012,
    User_Height_Not_Set = 1013,
    User_Location_Not_Set = 1014,
    User_Budget_Not_Set = 1015,
    MapsApi_Cannot_Locate_Place = 1016,
    No_Input_Model_Provided = 1017,

    // Internal Server Error >= 10000

    Legacy_Voorraden_Have_Different_FiliaalIds = 10000,
    Fiets_Cannot_Be_Refetched = 10001,
    MapsApi_Cannot_Be_Reached = 10002,
}