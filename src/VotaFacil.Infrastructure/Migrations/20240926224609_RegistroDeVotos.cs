using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotaFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RegistroDeVotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "opcao_voto_id",
                table: "voto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "opcao_voto_id",
                table: "voto",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
