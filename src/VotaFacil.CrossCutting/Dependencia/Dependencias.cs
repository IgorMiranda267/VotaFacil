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
            var connectionString = "Host=SharedPostgreSQL01A.back4app.com;Port=5433;Database=7b8a048fad4244579bd24c083cc2c8ad;Username=c1TkhEyWoo;Password=UPshUKQu63deRGaRrK5TuA84;";

            services.AddDbContext<VotacaoContext>(options =>
                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly(typeof(VotacaoContext).Assembly.FullName)));

            // Testar a conexão
            TestarConexao(connectionString);
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

        private static void TestarConexao(string connectionString)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Conexão com o banco de dados bem-sucedida.");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
            }
        }
    }
}
