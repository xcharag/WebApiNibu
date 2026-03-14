using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiNibu.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class ExpandQrAccessMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "QrAccess",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "QrAccess",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "QrAccess",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "QrAccess",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "QrAccess",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "WasUpsaStudent",
                table: "QrAccess",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "QrAccess");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "QrAccess");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "QrAccess");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "QrAccess");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "QrAccess");

            migrationBuilder.DropColumn(
                name: "WasUpsaStudent",
                table: "QrAccess");
        }
    }
}
