using System.Collections.Generic;

namespace Fietsenwinkel.Database.Models;

#pragma warning disable CS8618

internal class FiliaalModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<VoorraadModel> Voorraden { get; set; } = [];
}