using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Hesabdar.Migrations
{
    public partial class DealsPriceedit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Deal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15, 3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Deal",
                type: "decimal(15, 3)",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
