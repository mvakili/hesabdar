using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Hesabdar.Migrations
{
    public partial class testtt3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Deal_DealId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_DealId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "DealId",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "DealPaymentId",
                table: "Deal",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DealPriceId",
                table: "Deal",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deal_DealPaymentId",
                table: "Deal",
                column: "DealPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_DealPriceId",
                table: "Deal",
                column: "DealPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deal_Payment_DealPaymentId",
                table: "Deal",
                column: "DealPaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deal_Payment_DealPriceId",
                table: "Deal",
                column: "DealPriceId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deal_Payment_DealPaymentId",
                table: "Deal");

            migrationBuilder.DropForeignKey(
                name: "FK_Deal_Payment_DealPriceId",
                table: "Deal");

            migrationBuilder.DropIndex(
                name: "IX_Deal_DealPaymentId",
                table: "Deal");

            migrationBuilder.DropIndex(
                name: "IX_Deal_DealPriceId",
                table: "Deal");

            migrationBuilder.DropColumn(
                name: "DealPaymentId",
                table: "Deal");

            migrationBuilder.DropColumn(
                name: "DealPriceId",
                table: "Deal");

            migrationBuilder.AddColumn<int>(
                name: "DealId",
                table: "Payment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_DealId",
                table: "Payment",
                column: "DealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Deal_DealId",
                table: "Payment",
                column: "DealId",
                principalTable: "Deal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
