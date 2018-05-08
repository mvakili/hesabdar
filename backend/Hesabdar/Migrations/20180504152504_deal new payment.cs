using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Hesabdar.Migrations
{
    public partial class dealnewpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deal_Payment_PaymentId",
                table: "Deal");

            migrationBuilder.DropIndex(
                name: "IX_Deal_PaymentId",
                table: "Deal");

            migrationBuilder.DropColumn(
                name: "PaymentId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "PaymentId",
                table: "Deal",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deal_PaymentId",
                table: "Deal",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Deal_Payment_PaymentId",
                table: "Deal",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
