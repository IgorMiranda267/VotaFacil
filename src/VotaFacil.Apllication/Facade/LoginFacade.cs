using VotaFacil.Domain.Interfaces;
using VotaFacil.Infrastructure.Repositorios;

namespace VotaFacil.Apllication.Controller
{
    public class LoginFacade
    {
        private readonly ILoginRepositorio _loginRepositorio;

        public LoginFacade(ILoginRepositorio login)
        {
            _loginRepositorio = login;
        }

        public async Task<(bool, string)> Login(string username, string password)
        {
            return await _loginRepositorio.Login(username, password);
        }

        public async Task Logout(string token)
        {
            await _loginRepositorio.Logout(token);
        }
    }
}
