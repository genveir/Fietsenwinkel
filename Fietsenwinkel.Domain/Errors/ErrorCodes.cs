namespace Fietsenwinkel.Domain.Errors;

public enum ErrorCodes
{
    None = 0,

    // Not Found

    Voorraad_Not_Found = 1,
    Filiaal_Not_Found = 2,
    No_Matching_Fiets_Found = 3,
    FietsType_Not_Found = 4,
    Fiets_Not_Found = 5,

    // Bad Request >= 1000

    Fiets_Has_No_Wheels = 1000,
    Fiets_Has_Too_Many_Wheels = 1001,

    FietsType_Value_Not_Set = 1100,
    FietsType_Invalid_Format = 1101,

    AantalWielen_Value_Not_Set = 1200,

    FrameMaat_Value_Not_Set = 1300,
    FrameMaat_Invalid = 1301,

    FietsId_Value_Not_Set = 1400,
    FietsId_Invalid_Format = 1401,

    Price_Value_Not_Set = 1500,

    FiliaalId_Value_Not_Set = 2000,
    FiliaalId_Invalid_Format = 2001,

    FiliaalName_Value_NotSet = 2101,
    FiliaalName_Invalid_Format = 2102,

    Money_Value_Not_Set = 5000,
    User_Too_Short = 5001,
    User_Too_Tall = 5002,
    User_Height_Not_Set = 5003,
    User_Location_Not_Set = 5004,
    User_Budget_Not_Set = 5005,
    MapsApi_Cannot_Locate_Place = 5006,
    No_Input_Model_Provided = 5007,

    // Internal Server Error >= 10000

    Legacy_Voorraden_Have_Different_FiliaalIds = 10000,
    Fiets_Cannot_Be_Refetched = 10001,
    MapsApi_Cannot_Be_Reached = 10002,
}