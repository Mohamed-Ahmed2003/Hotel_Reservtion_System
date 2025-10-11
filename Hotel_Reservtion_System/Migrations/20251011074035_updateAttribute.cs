using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Reservtion_System.Migrations
{
    /// <inheritdoc />
    public partial class updateAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "userID",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_userID",
                table: "Invoices",
                column: "userID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_userID",
                table: "Invoices",
                column: "userID",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Users_userID",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_userID",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "userID",
                table: "Invoices");
        }
    }
}
