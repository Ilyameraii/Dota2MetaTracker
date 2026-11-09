using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Context.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetaUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaUpdates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeroStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HeroId = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    WinCount = table.Column<int>(type: "integer", nullable: false),
                    MatchCount = table.Column<int>(type: "integer", nullable: false),
                    MetaUpdateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeroStats_MetaUpdates_MetaUpdateId",
                        column: x => x.MetaUpdateId,
                        principalTable: "MetaUpdates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeroStats_MetaUpdateId_HeroId_Rank_Role",
                table: "HeroStats",
                columns: new[] { "MetaUpdateId", "HeroId", "Rank", "Role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetaUpdates_DateTime",
                table: "MetaUpdates",
                column: "DateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeroStats");

            migrationBuilder.DropTable(
                name: "MetaUpdates");
        }
    }
}
