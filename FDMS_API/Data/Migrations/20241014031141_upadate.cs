using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS_API.Data.Migrations
{
    /// <inheritdoc />
    public partial class upadate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserTokens",
                type: "varchar(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                columns: new[] { "Email", "PasswordHash" },
                values: new object[] { "admin@vietjetair.com", "$2a$11$2RlvPrcY2RVQeDePJ/2e.OtUmobFh5OJDhFcM0voRLWW8tqn20DYq" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                columns: new[] { "Email", "PasswordHash" },
                values: new object[] { "Admin@gmail.com", "1234567890" });
        }
    }
}
