namespace Fietsenwinkel.Database.Models;

#pragma warning disable CS8618

internal class FietsModel
{
    public int Id { get; set; }

    public int FrameMaat { get; set; }

    public int AantalWielen { get; set; }

    public int Price { get; set; }

    public int VoorraadId { get; set; }

    public VoorraadModel Voorraad { get; set; }

    public int FietsTypeId { get; set; }

    public FietsTypeModel FietsType { get; set; }
}