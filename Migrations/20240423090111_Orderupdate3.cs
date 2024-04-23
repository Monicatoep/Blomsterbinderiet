using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blomsterbinderiet.Migrations
{
    /// <inheritdoc />
    public partial class Orderupdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_EmployeeID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeID",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_EmployeeID",
                table: "Orders",
                column: "EmployeeID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_EmployeeID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_EmployeeID",
                table: "Orders",
                column: "EmployeeID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
