using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotaFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CadastroEleicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voto_votacao_votacao_id",
                table: "voto");

            migrationBuilder.DropTable(
                name: "CandidatoModelVotacaoModel");

            migrationBuilder.DropTable(
                name: "votacao");

            migrationBuilder.CreateTable(
                name: "eleicao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eleicao", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CandidatoModelEleicaoModel",
                columns: table => new
                {
                    CandidatosId = table.Column<Guid>(type: "uuid", nullable: false),
                    VotacoesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatoModelEleicaoModel", x => new { x.CandidatosId, x.VotacoesId });
                    table.ForeignKey(
                        name: "FK_CandidatoModelEleicaoModel_candidato_CandidatosId",
                        column: x => x.CandidatosId,
                        principalTable: "candidato",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidatoModelEleicaoModel_eleicao_VotacoesId",
                        column: x => x.VotacoesId,
                        principalTable: "eleicao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidatoModelEleicaoModel_VotacoesId",
                table: "CandidatoModelEleicaoModel",
                column: "VotacoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_voto_eleicao_votacao_id",
                table: "voto",
                column: "votacao_id",
                principalTable: "eleicao",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voto_eleicao_votacao_id",
                table: "voto");

            migrationBuilder.DropTable(
                name: "CandidatoModelEleicaoModel");

            migrationBuilder.DropTable(
                name: "eleicao");

            migrationBuilder.CreateTable(
                name: "votacao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_votacao", x => x.id);
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
                name: "IX_CandidatoModelVotacaoModel_VotacoesId",
                table: "CandidatoModelVotacaoModel",
                column: "VotacoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_voto_votacao_votacao_id",
                table: "voto",
                column: "votacao_id",
                principalTable: "votacao",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
