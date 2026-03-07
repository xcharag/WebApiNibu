using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiNibu.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class MatchTwoParticipations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Participations_ParticipationId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Match_ParticipationId_StartDate",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "ParticipationId",
                table: "Matches",
                newName: "ParticipationBId");

            migrationBuilder.AddColumn<int>(
                name: "ParticipationAId",
                table: "Matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(
                """UPDATE "Matches" SET "ParticipationAId" = "ParticipationBId";""");

            migrationBuilder.CreateIndex(
                name: "IX_Match_ParticipationAId_StartDate",
                table: "Matches",
                columns: new[] { "ParticipationAId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Match_ParticipationBId",
                table: "Matches",
                column: "ParticipationBId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Participations_ParticipationAId",
                table: "Matches",
                column: "ParticipationAId",
                principalTable: "Participations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Participations_ParticipationBId",
                table: "Matches",
                column: "ParticipationBId",
                principalTable: "Participations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Participations_ParticipationAId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Participations_ParticipationBId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Match_ParticipationAId_StartDate",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Match_ParticipationBId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ParticipationAId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "ParticipationBId",
                table: "Matches",
                newName: "ParticipationId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_ParticipationId_StartDate",
                table: "Matches",
                columns: new[] { "ParticipationId", "StartDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Participations_ParticipationId",
                table: "Matches",
                column: "ParticipationId",
                principalTable: "Participations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
