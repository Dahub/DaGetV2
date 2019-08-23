using Microsoft.EntityFrameworkCore.Migrations;

namespace DaGetV2.Api.Migrations
{
    public partial class add_readonly_to_user_bank_account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                schema: "daget",
                table: "UserBankAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                schema: "daget",
                table: "UserBankAccount");
        }
    }
}
