using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            //var connectionString = "Server=localhost;Port=3306;Database=nome_do_banco;User=root;Password=sua_senha;";
            //var serverVersion = ServerVersion.AutoDetect(connectionString);

            //services.AddDbContext<VotacaoContext>(options =>
            //    options.UseMySql(connectionString, serverVersion,
            //        b => b.MigrationsAssembly(typeof(VotacaoContext).Assembly.FullName)));

            services.AddDbContext<VotacaoContext>(options =>
                options.UseInMemoryDatabase("VotacaoDatabase"));
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
        }

        private static void AutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
