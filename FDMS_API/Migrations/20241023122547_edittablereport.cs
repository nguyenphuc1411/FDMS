using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS_API.Migrations
{
    /// <inheritdoc />
    public partial class edittablereport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdminUpload",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "VersionNumber",
                table: "VersionDocuments",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VersionDocuments",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Confirmed_At",
                table: "Reports",
                newName: "ReportedAt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Documents",
                newName: "Title");

            migrationBuilder.AlterColumn<decimal>(
                name: "Version",
                table: "Documents",
                type: "decimal(2,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$aK4.brJOBxctz3m5N8k6sOM1gExbJKeAfdTEYRh.hFA2fWvjpUFei");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Version",
                table: "VersionDocuments",
                newName: "VersionNumber");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "VersionDocuments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ReportedAt",
                table: "Reports",
                newName: "Confirmed_At");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Documents",
                newName: "Name");

            migrationBuilder.AlterColumn<decimal>(
                name: "Version",
                table: "Documents",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdminUpload",
                table: "Documents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$gA37cOVnvn6GSm0q2Sot8eR4vZYZUtrToeFrpgZx7D83c1JJx63l6");
        }
    }
}
