using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiNibu.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class NewRelationShipSchoolParticipation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participations_SchoolTable_SchoolTableId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_SchoolTableId",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "SchoolTableId",
                table: "Participations");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_SchoolId",
                table: "Participations",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_SchoolTable_SchoolId",
                table: "Participations",
                column: "SchoolId",
                principalTable: "SchoolTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participations_SchoolTable_SchoolId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_SchoolId",
                table: "Participations");

            migrationBuilder.AddColumn<int>(
                name: "SchoolTableId",
                table: "Participations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Participations_SchoolTableId",
                table: "Participations",
                column: "SchoolTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_SchoolTable_SchoolTableId",
                table: "Participations",
                column: "SchoolTableId",
                principalTable: "SchoolTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
