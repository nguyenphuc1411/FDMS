using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS_API.Data.Migrations
{
    /// <inheritdoc />
    public partial class mg3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemSettings_Users_UserID",
                table: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Confirmations");

            migrationBuilder.DropIndex(
                name: "IX_SystemSettings_UserID",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "Updated_At",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "SystemSettings");

            migrationBuilder.RenameColumn(
                name: "CanEdit",
                table: "Permissions",
                newName: "CanModify");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "DepartureTime",
                table: "Flights",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ArrivalTime",
                table: "Flights",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FlightDate",
                table: "Flights",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FlightID = table.Column<int>(type: "int", nullable: false),
                    SignatureURL = table.Column<string>(type: "varchar(255)", nullable: false),
                    Confirmed_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => new { x.UserID, x.FlightID });
                    table.ForeignKey(
                        name: "FK_Reports_Flights_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flights",
                        principalColumn: "FlightID");
                    table.ForeignKey(
                        name: "FK_Reports_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_FlightID",
                table: "Reports",
                column: "FlightID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropColumn(
                name: "FlightDate",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "CanModify",
                table: "Permissions",
                newName: "CanEdit");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_At",
                table: "SystemSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "SystemSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DepartureTime",
                table: "Flights",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalTime",
                table: "Flights",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.CreateTable(
                name: "Confirmations",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FlightID = table.Column<int>(type: "int", nullable: false),
                    Confirmed_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SignatureURL = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confirmations", x => new { x.UserID, x.FlightID });
                    table.ForeignKey(
                        name: "FK_Confirmations_Flights_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flights",
                        principalColumn: "FlightID");
                    table.ForeignKey(
                        name: "FK_Confirmations_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_UserID",
                table: "SystemSettings",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Confirmations_FlightID",
                table: "Confirmations",
                column: "FlightID");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSettings_Users_UserID",
                table: "SystemSettings",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
