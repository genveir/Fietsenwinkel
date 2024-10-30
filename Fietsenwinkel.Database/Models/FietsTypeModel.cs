using System.Collections.Generic;

namespace Fietsenwinkel.Database.Models;

#pragma warning disable CS8618

internal class FietsTypeModel
{
    public int Id { get; set; }
    public string TypeName { get; set; }

    public List<FietsModel> Fietsen { get; set; } = [];
}