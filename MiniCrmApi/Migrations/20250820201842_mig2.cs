using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCrmApi.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Deals",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "CustomerNotes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CustomerId",
                table: "Deals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_CustomerId",
                table: "CustomerNotes",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerNotes_Customers_CustomerId",
                table: "CustomerNotes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Customers_CustomerId",
                table: "Deals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerNotes_Customers_CustomerId",
                table: "CustomerNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Customers_CustomerId",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_CustomerId",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_CustomerNotes_CustomerId",
                table: "CustomerNotes");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerNotes");
        }
    }
}
