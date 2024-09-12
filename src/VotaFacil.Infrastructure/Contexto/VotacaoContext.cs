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
        public DbSet<VotacaoModel> Votacoes { get; set; }
        public DbSet<VotoModel> Votos { get; set; }
        public DbSet<LoginModel> Logins { get; set; }
        public DbSet<ContaEthereumModel> ContasEthereum { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuração da relação de um para um entre EleitorModel e LoginModel
            builder.Entity<EleitorModel>()
                .HasOne(e => e.Login)
                .WithOne(l => l.Eleitor)
                .HasForeignKey<LoginModel>(l => l.EleitorId);
        }
    }
}
