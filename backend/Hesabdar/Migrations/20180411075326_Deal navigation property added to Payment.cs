using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Hesabdar.Migrations
{
    public partial class DealnavigationpropertyaddedtoPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deal_PaymentId",
                table: "Deal");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_PaymentId",
                table: "Deal",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deal_PaymentId",
                table: "Deal");

            migrationBuilder.CreateIndex(
                name: "IX_Deal_PaymentId",
                table: "Deal",
                column: "PaymentId");
        }
    }
}
