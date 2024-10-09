using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS_API.Data.Migrations
{
    /// <inheritdoc />
    public partial class migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPermissions_GroupPermissions_GroupID",
                table: "DocumentPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentTypes_TypeID",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "Type_GroupPermissions");

            migrationBuilder.DropTable(
                name: "User_Groups");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "GroupPermissions");

            migrationBuilder.CreateTable(
                name: "Groups",
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
                    table.PrimaryKey("PK_Groups", x => x.GroupID);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Types",
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
                    table.PrimaryKey("PK_Types", x => x.TypeID);
                    table.ForeignKey(
                        name: "FK_Types_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => new { x.UserID, x.GroupID });
                    table.ForeignKey(
                        name: "FK_GroupUsers_Groups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "GroupID");
                    table.ForeignKey(
                        name: "FK_GroupUsers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false),
                    CanRead = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.TypeID, x.GroupID });
                    table.ForeignKey(
                        name: "FK_Permissions_Groups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "GroupID");
                    table.ForeignKey(
                        name: "FK_Permissions_Types_TypeID",
                        column: x => x.TypeID,
                        principalTable: "Types",
                        principalColumn: "TypeID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserID",
                table: "Groups",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupID",
                table: "GroupUsers",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_GroupID",
                table: "Permissions",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Types_UserID",
                table: "Types",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPermissions_Groups_GroupID",
                table: "DocumentPermissions",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Types_TypeID",
                table: "Documents",
                column: "TypeID",
                principalTable: "Types",
                principalColumn: "TypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentPermissions_Groups_GroupID",
                table: "DocumentPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Types_TypeID",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "GroupPermissions",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GroupName = table.Column<string>(type: "varchar(255)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_UserID",
                table: "DocumentTypes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissions_UserID",
                table: "GroupPermissions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Type_GroupPermissions_GroupID",
                table: "Type_GroupPermissions",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Groups_GroupID",
                table: "User_Groups",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentPermissions_GroupPermissions_GroupID",
                table: "DocumentPermissions",
                column: "GroupID",
                principalTable: "GroupPermissions",
                principalColumn: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentTypes_TypeID",
                table: "Documents",
                column: "TypeID",
                principalTable: "DocumentTypes",
                principalColumn: "TypeID");
        }
    }
}
