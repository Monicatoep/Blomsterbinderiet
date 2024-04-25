using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blomsterbinderiet.Migrations
{
    /// <inheritdoc />
    public partial class Porudct_disability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "Products");
        }
    }
}
