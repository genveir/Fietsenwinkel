﻿// <auto-generated />
using System;
using Fietsenwinkel.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fietsenwinkel.Database.Migrations
{
    [DbContext(typeof(FietsenwinkelContext))]
    partial class FietsenwinkelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Fietsenwinkel.Database.Models.FietsModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FietsTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FrameMaat")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VoorraadModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FietsTypeId");

                    b.HasIndex("VoorraadModelId");

                    b.ToTable("Fietsen");
                });

            modelBuilder.Entity("Fietsenwinkel.Database.Models.FietsTypeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FietsTypes");
                });

            modelBuilder.Entity("Fietsenwinkel.Database.Models.FiliaalModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Filialen");
                });

            modelBuilder.Entity("Fietsenwinkel.Database.Models.VoorraadModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FiliaalId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FiliaalId");

                    b.ToTable("Voorraden");
                });

            modelBuilder.Entity("Fietsenwinkel.Database.Models.FietsModel", b =>
                {
                    b.HasOne("Fietsenwinkel.Database.Models.FietsTypeModel", "FietsType")
                        .WithMany("Fietsen")
                        .HasForeignKey("FietsTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fietsenwinkel.Database.Models.VoorraadModel", null)
                        .WithMany("Fietsen")
                        .HasForeignKey("VoorraadModelId");

                    b.Navigation("FietsType");
                });

            modelBuilder.Entity("Fietsenwinkel.Database.Models.VoorraadModel", b =>
                {
                    b.HasOne("Fietsenwinkel.Database.Models.FiliaalModel", "Filiaal")
                        .WithMany("Voorraden")
                        .HasForeignKey("FiliaalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Filiaal");
                });

            modelBuilder.Entity("Fietsenwinkel.Database.Models.FietsTypeModel", b =>
                {
                    b.Navigation("Fietsen");
                });

            modelBuilder.Entity("Fietsenwinkel.Database.Models.FiliaalModel", b =>
                {
                    b.Navigation("Voorraden");
                });

            modelBuilder.Entity("Fietsenwinkel.Database.Models.VoorraadModel", b =>
                {
                    b.Navigation("Fietsen");
                });
#pragma warning restore 612, 618
        }
    }
}
