namespace Fietsenwinkel.Database.Models;

#pragma warning disable CS8618

internal class FietsModel
{
    public int Id { get; set; }

    public int FrameMaat { get; set; }

    public int FietsTypeId { get; set; }

    public FietsTypeModel FietsType { get; set; }
}