using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogWeb.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBirthDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Authors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                table: "Authors",
                type: "date",
                nullable: true);
        }
    }
}
