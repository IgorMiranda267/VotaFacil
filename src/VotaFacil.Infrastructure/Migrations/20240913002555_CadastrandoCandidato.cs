using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotaFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CadastrandoCandidato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "candidato_id",
                table: "voto",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "candidato",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    foto = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidato", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CandidatoModelVotacaoModel",
                columns: table => new
                {
                    CandidatosId = table.Column<Guid>(type: "uuid", nullable: false),
                    VotacoesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatoModelVotacaoModel", x => new { x.CandidatosId, x.VotacoesId });
                    table.ForeignKey(
                        name: "FK_CandidatoModelVotacaoModel_candidato_CandidatosId",
                        column: x => x.CandidatosId,
                        principalTable: "candidato",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidatoModelVotacaoModel_votacao_VotacoesId",
                        column: x => x.VotacoesId,
                        principalTable: "votacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_voto_candidato_id",
                table: "voto",
                column: "candidato_id");

            migrationBuilder.CreateIndex(
                name: "IX_CandidatoModelVotacaoModel_VotacoesId",
                table: "CandidatoModelVotacaoModel",
                column: "VotacoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_voto_candidato_candidato_id",
                table: "voto",
                column: "candidato_id",
                principalTable: "candidato",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voto_candidato_candidato_id",
                table: "voto");

            migrationBuilder.DropTable(
                name: "CandidatoModelVotacaoModel");

            migrationBuilder.DropTable(
                name: "candidato");

            migrationBuilder.DropIndex(
                name: "IX_voto_candidato_id",
                table: "voto");

            migrationBuilder.DropColumn(
                name: "candidato_id",
                table: "voto");
        }
    }
}
