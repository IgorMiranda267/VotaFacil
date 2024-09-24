using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotaFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelacaoEntreEleicaoCandidato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidatoModelEleicaoModel");

            migrationBuilder.CreateTable(
                name: "EleicaoCandidato",
                columns: table => new
                {
                    EleicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidatoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EleicaoCandidato", x => new { x.EleicaoId, x.CandidatoId });
                    table.ForeignKey(
                        name: "FK_EleicaoCandidato_Candidato",
                        column: x => x.CandidatoId,
                        principalTable: "candidato",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EleicaoCandidato_Eleicao",
                        column: x => x.EleicaoId,
                        principalTable: "eleicao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EleicaoCandidato_CandidatoId",
                table: "EleicaoCandidato",
                column: "CandidatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EleicaoCandidato");

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
        }
    }
}
