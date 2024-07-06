using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogWeb.Migrations
{
    /// <inheritdoc />
    public partial class RemovedFileType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileType",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
