namespace DaGetV2.Api.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class init : Migration
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
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false),
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
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FK_BankAccountType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Wording = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Wording = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    FK_BankAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FK_User = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FK_BankAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsTransfert = table.Column<bool>(type: "bit", nullable: false),
                    FK_BankAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FK_OperationType = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FK_OperationFrom = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FK_OperationTo = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.InsertData(
                schema: "daget",
                table: "BankAccountType",
                columns: new[] { "Id", "CreationDate", "ModificationDate", "Wording" },
                values: new object[] { new Guid("15f2a0f2-71f0-4823-8798-77cfa5752014"), new DateTime(2019, 9, 1, 13, 11, 6, 42, DateTimeKind.Local).AddTicks(8855), new DateTime(2019, 9, 1, 13, 11, 6, 45, DateTimeKind.Local).AddTicks(6591), "Courant" });

            migrationBuilder.InsertData(
                schema: "daget",
                table: "BankAccountType",
                columns: new[] { "Id", "CreationDate", "ModificationDate", "Wording" },
                values: new object[] { new Guid("c146e49e-5884-4174-81f3-e26a5f2cf8cd"), new DateTime(2019, 9, 1, 13, 11, 6, 45, DateTimeKind.Local).AddTicks(7648), new DateTime(2019, 9, 1, 13, 11, 6, 45, DateTimeKind.Local).AddTicks(7673), "Epargne" });

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
