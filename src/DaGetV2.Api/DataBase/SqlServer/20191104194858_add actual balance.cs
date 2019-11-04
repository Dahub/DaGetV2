using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaGetV2.Api.DataBase.SqlServer
{
    public partial class addactualbalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ActualBalance",
                schema: "daget",
                table: "BankAccount",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualBalance",
                schema: "daget",
                table: "BankAccount");
        }
    }
}
