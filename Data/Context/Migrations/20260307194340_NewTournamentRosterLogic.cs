using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApiNibu.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class NewTournamentRosterLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rosters_personTable_SchoolStudentId",
                table: "Rosters");

            migrationBuilder.RenameColumn(
                name: "SchoolStudentId",
                table: "Rosters",
                newName: "TournamentRosterId");

            migrationBuilder.RenameIndex(
                name: "IX_Rosters_SchoolStudentId",
                table: "Rosters",
                newName: "IX_Rosters_TournamentRosterId");

            migrationBuilder.CreateTable(
                name: "TournamentRosters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SchoolStudentId = table.Column<int>(type: "integer", nullable: false),
                    TournamentId = table.Column<int>(type: "integer", nullable: false),
                    SchoolId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentRosters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentRosters_SchoolTable_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentRosters_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentRosters_personTable_SchoolStudentId",
                        column: x => x.SchoolStudentId,
                        principalTable: "personTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRosters_SchoolId",
                table: "TournamentRosters",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRosters_SchoolStudentId",
                table: "TournamentRosters",
                column: "SchoolStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRosters_TournamentId",
                table: "TournamentRosters",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rosters_TournamentRosters_TournamentRosterId",
                table: "Rosters",
                column: "TournamentRosterId",
                principalTable: "TournamentRosters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rosters_TournamentRosters_TournamentRosterId",
                table: "Rosters");

            migrationBuilder.DropTable(
                name: "TournamentRosters");

            migrationBuilder.RenameColumn(
                name: "TournamentRosterId",
                table: "Rosters",
                newName: "SchoolStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Rosters_TournamentRosterId",
                table: "Rosters",
                newName: "IX_Rosters_SchoolStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rosters_personTable_SchoolStudentId",
                table: "Rosters",
                column: "SchoolStudentId",
                principalTable: "personTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
