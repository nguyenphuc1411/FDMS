using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS_API.Data.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(11)", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(max)", nullable: false),
                    IsTerminated = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.TypeID);
                    table.ForeignKey(
                        name: "FK_DocumentTypes_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    POL = table.Column<string>(type: "varchar(150)", nullable: false),
                    POU = table.Column<string>(type: "varchar(150)", nullable: false),
                    AircraftID = table.Column<string>(type: "varchar(20)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightID);
                    table.ForeignKey(
                        name: "FK_Flights_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "GroupPermissions",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "varchar(255)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPermissions", x => x.GroupID);
                    table.ForeignKey(
                        name: "FK_GroupPermissions_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Theme = table.Column<int>(type: "int", nullable: false),
                    LogoURL = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsCaptchaRequired = table.Column<bool>(type: "bit", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SystemSettings_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Confirmations",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FlightID = table.Column<int>(type: "int", nullable: false),
                    SignatureURL = table.Column<string>(type: "varchar(255)", nullable: false),
                    Confirmed_At = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Version = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "varchar(255)", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FlightID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_TypeID",
                        column: x => x.TypeID,
                        principalTable: "DocumentTypes",
                        principalColumn: "TypeID");
                    table.ForeignKey(
                        name: "FK_Documents_Flights_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flights",
                        principalColumn: "FlightID");
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Type_GroupPermissions",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false),
                    Permission = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_GroupPermissions", x => new { x.TypeID, x.GroupID });
                    table.ForeignKey(
                        name: "FK_Type_GroupPermissions_DocumentTypes_TypeID",
                        column: x => x.TypeID,
                        principalTable: "DocumentTypes",
                        principalColumn: "TypeID");
                    table.ForeignKey(
                        name: "FK_Type_GroupPermissions_GroupPermissions_GroupID",
                        column: x => x.GroupID,
                        principalTable: "GroupPermissions",
                        principalColumn: "GroupID");
                });

            migrationBuilder.CreateTable(
                name: "User_Groups",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Groups", x => new { x.UserID, x.GroupID });
                    table.ForeignKey(
                        name: "FK_User_Groups_GroupPermissions_GroupID",
                        column: x => x.GroupID,
                        principalTable: "GroupPermissions",
                        principalColumn: "GroupID");
                    table.ForeignKey(
                        name: "FK_User_Groups_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "DocumentPermissions",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentPermissions", x => new { x.DocumentID, x.GroupID });
                    table.ForeignKey(
                        name: "FK_DocumentPermissions_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentID");
                    table.ForeignKey(
                        name: "FK_DocumentPermissions_GroupPermissions_GroupID",
                        column: x => x.GroupID,
                        principalTable: "GroupPermissions",
                        principalColumn: "GroupID");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "IsTerminated", "Name", "PasswordHash", "Phone", "Role" },
                values: new object[] { 1, "Admin@gmail.com", false, "Admin default", "1234567890", "0898827656", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Confirmations_FlightID",
                table: "Confirmations",
                column: "FlightID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPermissions_GroupID",
                table: "DocumentPermissions",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FlightID",
                table: "Documents",
                column: "FlightID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TypeID",
                table: "Documents",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserID",
                table: "Documents",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_UserID",
                table: "DocumentTypes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_UserID",
                table: "Flights",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_UserID",
                table: "GroupPermissions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_UserID",
                table: "SystemSettings",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Type_GroupPermissions_GroupID",
                table: "Type_GroupPermissions",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Groups_GroupID",
                table: "User_Groups",
                column: "GroupID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Confirmations");

            migrationBuilder.DropTable(
                name: "DocumentPermissions");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Type_GroupPermissions");

            migrationBuilder.DropTable(
                name: "User_Groups");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "GroupPermissions");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
