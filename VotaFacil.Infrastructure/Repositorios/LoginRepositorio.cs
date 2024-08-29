using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Infrastructure.Repositorios
{
    public class LoginRepositorio : ILoginRepositorio
    {
        public Task<bool> Login(string username, string password)
        {
            return Task.FromResult(true);
        }

        public Task Logout()
        {
            return Task.CompletedTask;
        }
    }
}
