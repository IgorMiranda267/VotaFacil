using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotaFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TamnhoCampo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "chave_publica",
                table: "eleitor",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "chave_privada",
                table: "eleitor",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "chave_publica",
                table: "eleitor",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096);

            migrationBuilder.AlterColumn<string>(
                name: "chave_privada",
                table: "eleitor",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096);
        }
    }
}
