using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blomsterbinderiet.Migrations
{
    /// <inheritdoc />
    public partial class updatedKeywordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Keywords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Keywords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
