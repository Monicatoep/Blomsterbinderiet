﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blomsterbinderiet.Migrations
{
    /// <inheritdoc />
    public partial class Orderupdate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ProductPrice",
                table: "OrderLines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "OrderLines");
        }
    }
}
