using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Hesabdar.Migrations
{
    public partial class testtt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Dealer_PayeeId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Dealer_PayerId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PayeeId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PayerId",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "Payee",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Payer",
                table: "Payment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Payee",
                table: "Payment",
                column: "Payee");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Payer",
                table: "Payment",
                column: "Payer");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Dealer_Payee",
                table: "Payment",
                column: "Payee",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Dealer_Payer",
                table: "Payment",
                column: "Payer",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Dealer_Payee",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Dealer_Payer",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_Payee",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_Payer",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Payee",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Payer",
                table: "Payment");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PayeeId",
                table: "Payment",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PayerId",
                table: "Payment",
                column: "PayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Dealer_PayeeId",
                table: "Payment",
                column: "PayeeId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Dealer_PayerId",
                table: "Payment",
                column: "PayerId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
