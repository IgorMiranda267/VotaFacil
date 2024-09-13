using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infra.Data.Contexto;

namespace VotaFacil.Infrastructure.Repositorios
{
    public class LoginRepositorio : ILoginRepositorio
    {
        private readonly VotacaoContext _contexto;

        public LoginRepositorio(VotacaoContext contexto)
        {
            _contexto = contexto;
        }

        public async  Task<bool> Login(string username, string password)
        {
            var user = await _contexto.Set<LoginModel>()
                                      .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user != null;
        }

        public Task Logout()
        {
            return Task.CompletedTask;
        }
    }
}
