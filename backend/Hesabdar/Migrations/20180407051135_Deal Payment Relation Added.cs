using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hesabdar.Migrations
{
    public partial class DealPaymentRelationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Deal",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(15, 3)", nullable: false),
                    PayeeId = table.Column<int>(nullable: false),
                    PayerId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Dealer_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Dealer_PayerId",
                        column: x => x.PayerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deal_PaymentId",
                table: "Deal",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PayeeId",
                table: "Payment",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PayerId",
                table: "Payment",
                column: "PayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deal_Payment_PaymentId",
                table: "Deal",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deal_Payment_PaymentId",
                table: "Deal");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Deal_PaymentId",
                table: "Deal");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Deal");
        }
    }
}
