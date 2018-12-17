using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MinecraftServerScanner.Server.Migrations
{
    public partial class CreateScannableNetworkBlocksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScannableNetworkBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CIDR = table.Column<string>(nullable: true, maxLength: 20),
                    Created = table.Column<DateTime>(nullable: false),
                    State = table.Column<Byte>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScannableNetworkBlock", x => x.Id);
                    table.UniqueConstraint("UK_ScannableNetworkBlock", x => x.CIDR);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScannableNetworkBlocks");
        }
    }
}
