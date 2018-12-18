using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MinecraftServerScanner.Server.Migrations
{
    public partial class CreateMinecraftServersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MinecraftServers",
                columns: table => new
                {
                    Id = table.Column<Int32>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Host = table.Column<String>(nullable: true, maxLength: 16),
                    Port = table.Column<Int16>(nullable: false),
                    Online = table.Column<Boolean>(nullable: false),
                    Data = table.Column<String>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Scanned = table.Column<DateTime>(nullable: true),
                    LastOnline = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinecraftServer", x => x.Id);
                    table.UniqueConstraint("UK_MinecraftServer", x => new { x.Host, x.Port });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinecraftServers");
        }
    }
}
