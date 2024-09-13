using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using VotaFacil.Apllication.Controller;
using VotaFacil.Apllication.Facade;
using VotaFacil.Apllication.Mapper;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infra.Data.Contexto;
using VotaFacil.Infra.Data.Repositorios;
using VotaFacil.Infrastructure.Repositorios;

namespace VotaFacil.Infra.CrossCutting.Dependencia
{
    public static class Dependencias
    {
        public static IServiceCollection AddDependencyResolver(this IServiceCollection services, IConfiguration configuration)
        {
            ConfiguracaoBaseDados(services, configuration);
            Repositorios(services);
            Facade(services);
            AutoMapper(services);
            return services;
        }

        private static void ConfiguracaoBaseDados(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<VotacaoContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL"), b => b.MigrationsAssembly("VotaFacil.Infrastructure")));
        }

        private static void Repositorios(IServiceCollection services)
        {
            services.AddScoped<IEleitorRepositorio, EleitorRepositorio>();
            services.AddScoped<IVotacaoRepositorio, VotacaoRepositorio>();
            services.AddScoped<ILoginRepositorio, LoginRepositorio>();
        }

        private static void Facade(IServiceCollection services)
        {
            services.AddScoped<LoginFacade>();
            services.AddScoped<EleitorFacade>();
            services.AddScoped<VotacaoFacade>();
        }

        private static void AutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
