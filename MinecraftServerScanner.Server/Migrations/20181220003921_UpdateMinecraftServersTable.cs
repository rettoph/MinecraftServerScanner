using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MinecraftServerScanner.Server.Migrations
{
    public partial class UpdateMinecraftServersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<String>(
                name: "icon",
                table: "MinecraftServers",
                nullable: true);

            migrationBuilder.AddColumn<String>(
                name: "version",
                table: "MinecraftServers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "MinecraftServers");

            migrationBuilder.DropColumn(
                name: "version",
                table: "MinecraftServers");
        }
    }
}
