using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;

namespace VotaFacil.Infra.Data.Contexto
{
    public class VotacaoContext : DbContext
    {
        public VotacaoContext(DbContextOptions<VotacaoContext> options) : base(options)
        { }

        public DbSet<VotanteModel> Votantes { get; set; }
        public DbSet<VotacaoModel> Votacoes { get; set; }
        public DbSet<OpcaoVotoModel> OpcoesVoto { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(VotacaoContext).Assembly);
        }
    }
}
