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

            // Configurações para EleitorModel
            builder.Entity<EleitorModel>()
                .HasOne(e => e.ContaEthereum)
                .WithOne()
                .HasForeignKey<EleitorModel>(e => e.ContaEthereumId);

            //builder.Entity<EleitorModel>()
            //    .HasMany(e => e.Votacoes)
            //    .WithOne()
            //    .HasForeignKey(v => v.EleitorId);

            // Configurações para VotacaoModel
            builder.Entity<VotacaoModel>()
                .HasMany(v => v.OpcoesDeVoto)
                .WithOne()
                .HasForeignKey(v => v.VotacaoId);

            // Configurações para VotoModel
            builder.Entity<VotoModel>()
                .HasOne(v => v.Eleitor)
                .WithMany()
                .HasForeignKey(v => v.EleitorId);

            builder.Entity<VotoModel>()
                .HasOne(v => v.Votacao)
                .WithMany(v => v.OpcoesDeVoto)
                .HasForeignKey(v => v.VotacaoId);

            // Configurações para LoginModel
            builder.Entity<LoginModel>()
                .HasOne(l => l.Eleitor)
                .WithMany()
                .HasForeignKey(l => l.EleitorId);

            // Configurações para ContaEthereumModel
            builder.Entity<ContaEthereumModel>()
                .HasKey(c => c.Id);
        }
    }
}
