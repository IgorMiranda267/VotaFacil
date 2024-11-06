using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotaFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutenticacaoDoisFatores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "codigo_verificacao",
                table: "eleitor",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "eleitor",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigo_verificacao",
                table: "eleitor");

            migrationBuilder.DropColumn(
                name: "email",
                table: "eleitor");
        }
    }
}
