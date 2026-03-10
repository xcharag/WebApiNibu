using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiNibu.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class TournamentRosterInlineNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentRosters_personTable_SchoolStudentId",
                table: "TournamentRosters");

            migrationBuilder.DropIndex(
                name: "IX_TournamentRosters_SchoolStudentId",
                table: "TournamentRosters");

            migrationBuilder.DropColumn(
                name: "SchoolStudentId",
                table: "TournamentRosters");

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "TournamentRosters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "TournamentRosters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "TournamentRosters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaternalName",
                table: "TournamentRosters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "TournamentRosters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "TournamentRosters");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "TournamentRosters");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "TournamentRosters");

            migrationBuilder.DropColumn(
                name: "MaternalName",
                table: "TournamentRosters");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "TournamentRosters");

            migrationBuilder.AddColumn<int>(
                name: "SchoolStudentId",
                table: "TournamentRosters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRosters_SchoolStudentId",
                table: "TournamentRosters",
                column: "SchoolStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentRosters_personTable_SchoolStudentId",
                table: "TournamentRosters",
                column: "SchoolStudentId",
                principalTable: "personTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
