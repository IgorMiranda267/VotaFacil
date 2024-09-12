using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VotaFacil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conta_ethereum",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    address = table.Column<string>(type: "character varying(42)", maxLength: 42, nullable: false),
                    private_key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conta_ethereum", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "eleitor",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    identificador = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    endereco_ethereum = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eleitor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "votacao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_votacao", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "login",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ultimo_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    expiracao_token = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eleitor_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login", x => x.id);
                    table.ForeignKey(
                        name: "FK_login_eleitor_eleitor_id",
                        column: x => x.eleitor_id,
                        principalTable: "eleitor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "voto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    eleitor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    opcao_voto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    votacao_id = table.Column<Guid>(type: "uuid", nullable: false),
                    data_hora_voto = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    hash_anterior = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    hash_atual = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voto", x => x.id);
                    table.ForeignKey(
                        name: "FK_voto_eleitor_eleitor_id",
                        column: x => x.eleitor_id,
                        principalTable: "eleitor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_voto_votacao_votacao_id",
                        column: x => x.votacao_id,
                        principalTable: "votacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_login_eleitor_id",
                table: "login",
                column: "eleitor_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voto_eleitor_id",
                table: "voto",
                column: "eleitor_id");

            migrationBuilder.CreateIndex(
                name: "IX_voto_votacao_id",
                table: "voto",
                column: "votacao_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conta_ethereum");

            migrationBuilder.DropTable(
                name: "login");

            migrationBuilder.DropTable(
                name: "voto");

            migrationBuilder.DropTable(
                name: "eleitor");

            migrationBuilder.DropTable(
                name: "votacao");
        }
    }
}
