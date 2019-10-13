using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaGetV2.Api.Migrations
{
    public partial class add_wording_to_operation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Wording",
                schema: "daget",
                table: "Operation",
                type: "nvarchar(512)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wording",
                schema: "daget",
                table: "Operation");
        }
    }
}
