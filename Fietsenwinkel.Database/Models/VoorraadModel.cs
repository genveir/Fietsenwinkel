using System.Collections.Generic;

namespace Fietsenwinkel.Database.Models;

#pragma warning disable CS8618

internal class VoorraadModel
{
    public int Id { get; set; }

    public int FiliaalId { get; set; }

    public FiliaalModel Filiaal { get; set; }

    public List<FietsModel> Fietsen { get; set; } = [];
}