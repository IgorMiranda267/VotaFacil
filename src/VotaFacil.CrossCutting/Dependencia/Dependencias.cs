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
            services.AddSingleton<IJwtTokenValidator>(provider => new JwtTokenValidator(secretKey, tokenRevocationDuration));
        }

        private static void AmazonS3(IServiceCollection services, IConfiguration configuration)
        {
            // Configurar o cliente S3
            //var AccessKey = Environment.GetEnvironmentVariable("SECRET_KEY_JWT");
            //var SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY_JWT");
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
