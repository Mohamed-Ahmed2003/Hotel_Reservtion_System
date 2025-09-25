using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Reservtion_System.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    location = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    confirmPassword = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    isApproved = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    roomType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    roomCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    availability = table.Column<bool>(type: "bit", maxLength: 20, nullable: false),
                    price = table.Column<double>(type: "float", nullable: true),
                    branchID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rooms_Branches_branchID",
                        column: x => x.branchID,
                        principalTable: "Branches",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Notifcations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    message = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifcations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notifcations_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    roomid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    checkIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    checkOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_roomid",
                        column: x => x.roomid,
                        principalTable: "Rooms",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Bookings_Users_userid",
                        column: x => x.userid,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bookID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    totalAmount = table.Column<double>(type: "float", nullable: true),
                    discount = table.Column<double>(type: "float", nullable: true),
                    tax = table.Column<double>(type: "float", nullable: true),
                    finalAmount = table.Column<double>(type: "float", nullable: true),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.id);
                    table.ForeignKey(
                        name: "FK_Invoices_Bookings_bookID",
                        column: x => x.bookID,
                        principalTable: "Bookings",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_roomid",
                table: "Bookings",
                column: "roomid");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_userid",
                table: "Bookings",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_bookID",
                table: "Invoices",
                column: "bookID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifcations_userID",
                table: "Notifcations",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_branchID",
                table: "Rooms",
                column: "branchID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Notifcations");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
