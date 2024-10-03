// C#
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
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
using VotaFacil.Infrastructure.Service;

namespace VotaFacil.Infra.CrossCutting.Dependencia
{
    public static class Dependencias
    {
        public static IServiceCollection AddDependencyResolver(this IServiceCollection services, IConfiguration configuration)
        {
            Facade(services);
            Service(services);
            AmazonS3(services, configuration);
            AutoMapper(services);
            Repositorios(services);
            ConfiguracaoBaseDados(services, configuration);
            return services;
        }

        private static void ConfiguracaoBaseDados(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            services.AddDbContext<VotacaoContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL"), b => b.MigrationsAssembly("VotaFacil.Infrastructure")));
        }

        private static void Repositorios(IServiceCollection services)
        {
            services.AddScoped<IEleitorRepositorio, EleitorRepositorio>();
            services.AddScoped<IEleicaoRepositorio, EleicaoRepositorio>();
            services.AddScoped<ILoginRepositorio, LoginRepositorio>();
            services.AddScoped<IVotoRepositorio, VotoRepositorio>();
            services.AddScoped<IEthereumService, EthereumService>();
        }

        private static void Facade(IServiceCollection services)
        {
            services.AddScoped<LoginFacade>();
            services.AddScoped<EleitorFacade>();
            services.AddScoped<EleicaoFacade>();
            services.AddScoped<VotoFacade>();
        }

        private static void AutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }

        private static void Service(IServiceCollection services)
        {
            var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY_JWT");
            var tokenRevocationDuration = TimeSpan.FromHours(1);
            services.AddSingleton<IJwtTokenService>(provider => new JwtTokenValidator(secretKey, tokenRevocationDuration));
        }

        private static void Infura(IServiceCollection services)
        {
            var id = Environment.GetEnvironmentVariable("INFURA_ETHEREUM_ID_ACCOUNT") ?? throw new ArgumentException("INFURA_ETHEREUM_ID_ACCOUNT não pode ser nulo.");
            var carteira = Environment.GetEnvironmentVariable("META_MASK_WALLET_ADDRESS") ?? throw new ArgumentException("META_MASK_WALLET_ADDRESS não pode ser nulo.");
            var chavePrivada = Environment.GetEnvironmentVariable("INFURA_ETHEREUM_PRIVATE_KEY") ?? throw new ArgumentException("INFURA_ETHEREUM_PRIVATE_KEY não pode ser nulo.");
            var contratoInteligente = Environment.GetEnvironmentVariable("INFURA_ETHEREUM_CONTRACT_ADDRESS") ?? throw new ArgumentException("INFURA_ETHEREUM_CONTRACT_ADDRESS não pode ser nulo.");

            services.AddScoped<EthereumService>(provider => new EthereumService(
                $"https://mainnet.infura.io/v3/{id}",
                contratoInteligente,
                carteira,
                chavePrivada
                ));
        }

        private static void AmazonS3(IServiceCollection services, IConfiguration configuration)
        {
            // Configurar o cliente S3
            //var awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            //var awsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Credentials = new BasicAWSCredentials(
                configuration["AWS:AccessKey"],
                configuration["AWS:SecretKey"]
            );
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();
        }
    }
}
