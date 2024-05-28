using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImagesfieldname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Images",
                newName: "FileUrlPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileUrlPath",
                table: "Images",
                newName: "FilePath");
        }
    }
}
