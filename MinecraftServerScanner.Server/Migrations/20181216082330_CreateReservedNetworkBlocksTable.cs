using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MinecraftServerScanner.Server.Migrations
{
    public partial class CreateReservedNetworkBlocksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservedNetworkBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CIDR = table.Column<string>(nullable: true, maxLength: 20),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedNetworkBlocks", x => x.Id);
                    table.UniqueConstraint("UK_ReservedNetworkBlocks", x => x.CIDR);
                });

            /**
             * Seed the reserved blocks table with data found here:
             * https://en.wikipedia.org/wiki/Reserved_IP_addresses
             */
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "0.0.0.0/8", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "10.0.0.0/8", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "100.64.0.0/10", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "127.0.0.0/8", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "169.254.0.0/16", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "172.16.0.0/12", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "192.0.0.0/24", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "192.0.2.0/24", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "192.88.99.0/24", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "192.168.0.0/16", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "198.18.0.0/15", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "198.51.100.0/24", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "203.0.113.0/24", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "224.0.0.0/4", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "240.0.0.0/4", DateTime.Now });
            migrationBuilder.InsertData(
                table: "ReservedNetworkBlocks",
                columns: new[] { "CIDR", "Created" },
                values: new object[] { "255.255.255.255/32", DateTime.Now });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservedNetworkBlocks");
        }
    }
}
