using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Hesabdar.Migrations
{
    public partial class DealItemnavigationPropertiesset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealItem_Deal_DealId",
                table: "DealItem");

            migrationBuilder.AlterColumn<int>(
                name: "DealId",
                table: "DealItem",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "DealItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DealItem_Deal_DealId",
                table: "DealItem",
                column: "DealId",
                principalTable: "Deal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealItem_Deal_DealId",
                table: "DealItem");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "DealItem");

            migrationBuilder.AlterColumn<int>(
                name: "DealId",
                table: "DealItem",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_DealItem_Deal_DealId",
                table: "DealItem",
                column: "DealId",
                principalTable: "Deal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
