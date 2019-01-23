using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DaGetV2.Api.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "daget");

            migrationBuilder.CreateTable(
                name: "BankAccountType",
                schema: "daget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Wording = table.Column<string>(type: "nvarchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "daget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastConnexionDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "daget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FK_BankAccountType = table.Column<int>(type: "integer", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccount_BankAccountType_FK_BankAccountType",
                        column: x => x.FK_BankAccountType,
                        principalSchema: "daget",
                        principalTable: "BankAccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperationType",
                schema: "daget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Wording = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    FK_BankAccount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationType_BankAccount_FK_BankAccount",
                        column: x => x.FK_BankAccount,
                        principalSchema: "daget",
                        principalTable: "BankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBankAccount",
                schema: "daget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FK_User = table.Column<int>(type: "integer", nullable: false),
                    FK_BankAccount = table.Column<int>(type: "integer", nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBankAccount_BankAccount_FK_BankAccount",
                        column: x => x.FK_BankAccount,
                        principalSchema: "daget",
                        principalTable: "BankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBankAccount_User_FK_User",
                        column: x => x.FK_User,
                        principalSchema: "daget",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                schema: "daget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsTransfert = table.Column<bool>(type: "bit", nullable: false),
                    FK_BankAccount = table.Column<int>(type: "integer", nullable: false),
                    FK_OperationType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operation_BankAccount_FK_BankAccount",
                        column: x => x.FK_BankAccount,
                        principalSchema: "daget",
                        principalTable: "BankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operation_OperationType_FK_OperationType",
                        column: x => x.FK_OperationType,
                        principalSchema: "daget",
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transfert",
                schema: "daget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FK_OperationFrom = table.Column<int>(type: "integer", nullable: false),
                    FK_OperationTo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfert", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfert_Operation_FK_OperationFrom",
                        column: x => x.FK_OperationFrom,
                        principalSchema: "daget",
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfert_Operation_FK_OperationTo",
                        column: x => x.FK_OperationTo,
                        principalSchema: "daget",
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_FK_BankAccountType",
                schema: "daget",
                table: "BankAccount",
                column: "FK_BankAccountType");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_FK_BankAccount",
                schema: "daget",
                table: "Operation",
                column: "FK_BankAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_FK_OperationType",
                schema: "daget",
                table: "Operation",
                column: "FK_OperationType");

            migrationBuilder.CreateIndex(
                name: "IX_OperationType_FK_BankAccount",
                schema: "daget",
                table: "OperationType",
                column: "FK_BankAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Transfert_FK_OperationFrom",
                schema: "daget",
                table: "Transfert",
                column: "FK_OperationFrom");

            migrationBuilder.CreateIndex(
                name: "IX_Transfert_FK_OperationTo",
                schema: "daget",
                table: "Transfert",
                column: "FK_OperationTo");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                schema: "daget",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBankAccount_FK_BankAccount",
                schema: "daget",
                table: "UserBankAccount",
                column: "FK_BankAccount");

            migrationBuilder.CreateIndex(
                name: "IX_UserBankAccount_FK_User",
                schema: "daget",
                table: "UserBankAccount",
                column: "FK_User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfert",
                schema: "daget");

            migrationBuilder.DropTable(
                name: "UserBankAccount",
                schema: "daget");

            migrationBuilder.DropTable(
                name: "Operation",
                schema: "daget");

            migrationBuilder.DropTable(
                name: "User",
                schema: "daget");

            migrationBuilder.DropTable(
                name: "OperationType",
                schema: "daget");

            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "daget");

            migrationBuilder.DropTable(
                name: "BankAccountType",
                schema: "daget");
        }
    }
}
