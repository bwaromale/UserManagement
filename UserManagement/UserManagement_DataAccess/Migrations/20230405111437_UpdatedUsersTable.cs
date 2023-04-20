using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement_DataAccess.Migrations
{
    public partial class UpdatedUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Users",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Users");
        }
    }
}
