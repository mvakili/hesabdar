using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Hesabdar.Migrations
{
    public partial class DealItemnavigationPropertiesMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerOne",
                table: "DealItem",
                type: "decimal(15, 3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "DealItem",
                type: "decimal(15, 3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_DealItem_MaterialId",
                table: "DealItem",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealItem_Material_MaterialId",
                table: "DealItem",
                column: "MaterialId",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealItem_Material_MaterialId",
                table: "DealItem");

            migrationBuilder.DropIndex(
                name: "IX_DealItem_MaterialId",
                table: "DealItem");

            migrationBuilder.DropColumn(
                name: "PricePerOne",
                table: "DealItem");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "DealItem");
        }
    }
}
