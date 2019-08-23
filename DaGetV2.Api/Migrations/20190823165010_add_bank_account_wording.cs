using Microsoft.EntityFrameworkCore.Migrations;

namespace DaGetV2.Api.Migrations
{
    public partial class add_bank_account_wording : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Wording",
                schema: "daget",
                table: "BankAccount",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wording",
                schema: "daget",
                table: "BankAccount");
        }
    }
}
