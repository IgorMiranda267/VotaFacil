using VotaFacil.Infra.Data.Contexto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infra.Data.Repositorios;
using VotaFacil.Infrastructure.Repositorios;
using VotaFacil.Apllication.Controller;

namespace VotaFacil.Infra.CrossCutting.Dependencia
{
    public static class Dependencias
    {
        public static IServiceCollection AddDependencyResolver(this IServiceCollection services, IConfiguration configuration)
        {
            ConfiguracaoBaseDados(services, configuration);
            Repositorios(services);

            return services;
        }

        private static void ConfiguracaoBaseDados(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VotacaoContext>(options => options.UseMySql(
                configuration.GetConnectionString("CONNECTION-STRING"),
                b => b.MigrationsAssembly(typeof(VotacaoContext).Assembly.FullName)));
        }

        private static void Repositorios(IServiceCollection services)
        {
            services.AddScoped<IVotanteRepositorio, VotanteRepositorio>();
            services.AddScoped<IVotacaoRepositorio, VotacaoRepositorio>();
            services.AddScoped<ILoginRepositorio, LoginRepositorio>();
            services.AddScoped<LoginFacade>();
        }
    }
}
