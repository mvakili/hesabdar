using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Hesabdar.Migrations
{
    public partial class DealsDealerKeysaddedtomodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deal_Dealer_BuyerId",
                table: "Deal");

            migrationBuilder.DropForeignKey(
                name: "FK_Deal_Dealer_SellerId",
                table: "Deal");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Deal",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "Deal",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Deal_Dealer_BuyerId",
                table: "Deal",
                column: "BuyerId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deal_Dealer_SellerId",
                table: "Deal",
                column: "SellerId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deal_Dealer_BuyerId",
                table: "Deal");

            migrationBuilder.DropForeignKey(
                name: "FK_Deal_Dealer_SellerId",
                table: "Deal");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Deal",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "Deal",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Deal_Dealer_BuyerId",
                table: "Deal",
                column: "BuyerId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deal_Dealer_SellerId",
                table: "Deal",
                column: "SellerId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
