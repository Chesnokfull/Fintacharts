using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintacharts_Data_Collection.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tickSize = table.Column<double>(type: "float", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    baseCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentsValuesTimely",
                columns: table => new
                {
                    t = table.Column<DateTime>(type: "datetime2", nullable: false),
                    o = table.Column<float>(type: "real", nullable: false),
                    h = table.Column<float>(type: "real", nullable: false),
                    l = table.Column<float>(type: "real", nullable: false),
                    c = table.Column<float>(type: "real", nullable: false),
                    v = table.Column<int>(type: "int", nullable: false),
                    instrumentid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_InstrumentsValuesTimely_Instruments_instrumentid",
                        column: x => x.instrumentid,
                        principalTable: "Instruments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Instrumentid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    exchange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    defaultOrderSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Providers_Instruments_Instrumentid",
                        column: x => x.Instrumentid,
                        principalTable: "Instruments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebSocketMessages",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    instrumentalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subscribe = table.Column<bool>(type: "bit", nullable: false),
                    kinds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    instrumentid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebSocketMessages", x => x.id);
                    table.ForeignKey(
                        name: "FK_WebSocketMessages_Instruments_instrumentid",
                        column: x => x.instrumentid,
                        principalTable: "Instruments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsValuesTimely_instrumentid",
                table: "InstrumentsValuesTimely",
                column: "instrumentid");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_Instrumentid",
                table: "Providers",
                column: "Instrumentid");

            migrationBuilder.CreateIndex(
                name: "IX_WebSocketMessages_instrumentid",
                table: "WebSocketMessages",
                column: "instrumentid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstrumentsValuesTimely");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "WebSocketMessages");

            migrationBuilder.DropTable(
                name: "Instruments");
        }
    }
}
