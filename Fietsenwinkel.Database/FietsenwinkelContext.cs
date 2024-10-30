using Fietsenwinkel.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fietsenwinkel.Database;

internal class FietsenwinkelContext : DbContext
{
    public string DbPath { get; }

    public DbSet<FiliaalModel> Filialen { get; set; }
    public DbSet<VoorraadModel> Voorraden { get; set; }
    public DbSet<FietsModel> Fietsen { get; set; }
    public DbSet<FietsTypeModel> FietsTypes { get; set; }

    public FietsenwinkelContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "fietsenwinkel.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
}