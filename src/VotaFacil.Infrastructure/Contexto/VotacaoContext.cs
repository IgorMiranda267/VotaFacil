using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Model;

namespace VotaFacil.Infra.Data.Contexto
{
    public class VotacaoContext : DbContext
    {
        public VotacaoContext(DbContextOptions<VotacaoContext> options) : base(options)
        { }

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

            // Configuração da relação de muitos para muitos entre EleicaoModel e CandidatoModel
            builder.Entity<EleicaoModel>()
                .HasMany(v => v.Candidatos)
                .WithMany(c => c.Votacoes);

            // Configuração da relação de um para muitos entre CandidatoModel e VotoModel
            builder.Entity<VotoModel>()
                .HasOne(v => v.Candidato)
                .WithMany(c => c.Votos)
                .HasForeignKey(v => v.CandidatoId);

            // Configuração da relação de um para muitos entre VotacaoModel e VotoModel
            builder.Entity<VotoModel>()
                .HasOne(v => v.Votacao)
                .WithMany(v => v.Votos)
                .HasForeignKey(v => v.VotacaoId);
        }
    }
}
