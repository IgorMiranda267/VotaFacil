using System.Numerics;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotaFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SmartContracts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "numero_bloco",
                table: "voto",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "block_number",
                table: "eleicao",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "contract_address",
                table: "eleicao",
                type: "character varying(42)",
                maxLength: 42,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "gas_used",
                table: "eleicao",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "transaction_hash",
                table: "eleicao",
                type: "character varying(66)",
                maxLength: 66,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HexBigInteger",
                columns: table => new
                {
                    HexValue = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<BigInteger>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HexBigInteger");

            migrationBuilder.DropColumn(
                name: "numero_bloco",
                table: "voto");

            migrationBuilder.DropColumn(
                name: "block_number",
                table: "eleicao");

            migrationBuilder.DropColumn(
                name: "contract_address",
                table: "eleicao");

            migrationBuilder.DropColumn(
                name: "gas_used",
                table: "eleicao");

            migrationBuilder.DropColumn(
                name: "transaction_hash",
                table: "eleicao");
        }
    }
}
