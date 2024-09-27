using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Model;

namespace VotaFacil.Infra.Data.Contexto
{
    public class VotacaoContext : DbContext
    {
        public VotacaoContext(DbContextOptions<VotacaoContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }

        public DbSet<EleitorModel> Eleitores { get; set; }
        public DbSet<EleicaoModel> Votacoes { get; set; }
        public DbSet<VotoModel> Votos { get; set; }
        public DbSet<LoginModel> Logins { get; set; }
        public DbSet<ContaEthereumModel> ContasEthereum { get; set; }
        public DbSet<CandidatoModel> Candidatos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuração da relação de um para um entre EleitorModel e LoginModel
            builder.Entity<EleitorModel>()
                .HasOne(e => e.Login)
                .WithOne(l => l.Eleitor)
                .HasForeignKey<LoginModel>(l => l.EleitorId);

            // Relação muitos-para-muitos entre EleicaoModel e CandidatoModel
            builder.Entity<EleicaoModel>()
                .HasMany(e => e.Candidatos)
                .WithMany(c => c.Votacoes)
                .UsingEntity<Dictionary<string, object>>(
                    "EleicaoCandidato", // Nome da tabela de junção
                    j => j.HasOne<CandidatoModel>()
                          .WithMany()
                          .HasForeignKey("CandidatoId")
                          .HasConstraintName("FK_EleicaoCandidato_Candidato")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<EleicaoModel>()
                          .WithMany()
                          .HasForeignKey("EleicaoId")
                          .HasConstraintName("FK_EleicaoCandidato_Eleicao")
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("EleicaoId", "CandidatoId");
                        j.ToTable("EleicaoCandidato");
                    }
                );

            /// Relação um-para-muitos entre CandidatoModel e VotoModel
            builder.Entity<VotoModel>()
                .HasOne(v => v.Candidato)
                .WithMany(c => c.Votos)
                .HasForeignKey(v => v.CandidatoId)
                .OnDelete(DeleteBehavior.Cascade); // Deletar votos quando o candidato for deletado

            // Configuração da relação de um para muitos entre EleicaoModel e VotoModel
            builder.Entity<VotoModel>()
                .HasOne(v => v.Votacao)           // Um voto tem uma eleição
                .WithMany(e => e.Votos)           // Uma eleição tem muitos votos
                .HasForeignKey(v => v.VotacaoId)  // Chave estrangeira no Voto
                .OnDelete(DeleteBehavior.Cascade); // Delete em cascata ao remover uma eleição
        }
    }
}
