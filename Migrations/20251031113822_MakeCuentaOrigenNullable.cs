using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLink.Migrations
{
    /// <inheritdoc />
    public partial class MakeCuentaOrigenNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transferencias_Cuentas_CuentaOrigenId",
                table: "Transferencias");

            migrationBuilder.AlterColumn<int>(
                name: "CuentaOrigenId",
                table: "Transferencias",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencias_Cuentas_CuentaOrigenId",
                table: "Transferencias",
                column: "CuentaOrigenId",
                principalTable: "Cuentas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transferencias_Cuentas_CuentaOrigenId",
                table: "Transferencias");

            migrationBuilder.AlterColumn<int>(
                name: "CuentaOrigenId",
                table: "Transferencias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencias_Cuentas_CuentaOrigenId",
                table: "Transferencias",
                column: "CuentaOrigenId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
