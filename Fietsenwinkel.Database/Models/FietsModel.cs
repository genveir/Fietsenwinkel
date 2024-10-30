namespace Fietsenwinkel.Database.Models;

internal class FietsModel
{
    public int Id { get; set; }

    public int FrameMaat { get; set; }

    public int FietsTypeId { get; set; }

    public FietsTypeModel? FietsType { get; set; }
}