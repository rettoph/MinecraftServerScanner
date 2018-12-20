﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinecraftServerScanner.Server;

namespace MinecraftServerScanner.Server.Migrations
{
    [DbContext(typeof(MinecraftContext))]
    [Migration("20181216082330_CreateReservedNetworkBlocksTable")]
    partial class CreateReservedNetworkBlocksTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MinecraftServerScanner.Server.Models.ReservedNetworkBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CIDR");

                    b.HasKey("Id");

                    b.ToTable("ReservedNetworkBlock");
                });
#pragma warning restore 612, 618
        }
    }
}
