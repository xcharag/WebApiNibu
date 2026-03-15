using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiNibu.Context.Core.Migrations
{
    /// <inheritdoc />
    public partial class comentarioycolegioenqr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "QrAccess",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolTableId",
                table: "QrAccess",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QrAccess_SchoolTableId",
                table: "QrAccess",
                column: "SchoolTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_QrAccess_SchoolTable_SchoolTableId",
                table: "QrAccess",
                column: "SchoolTableId",
                principalTable: "SchoolTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QrAccess_SchoolTable_SchoolTableId",
                table: "QrAccess");

            migrationBuilder.DropIndex(
                name: "IX_QrAccess_SchoolTableId",
                table: "QrAccess");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "QrAccess");

            migrationBuilder.DropColumn(
                name: "SchoolTableId",
                table: "QrAccess");
        }
    }
}
