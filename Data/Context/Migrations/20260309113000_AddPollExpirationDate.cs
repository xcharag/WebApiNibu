using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApiNibu.Data.Context;

#nullable disable

namespace WebApiNibu.Data.Context.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20260309113000_AddPollExpirationDate")]
    /// <inheritdoc />
    public partial class AddPollExpirationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Poll",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Poll");
        }
    }
}
