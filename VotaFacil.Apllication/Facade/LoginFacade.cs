using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Apllication.Controller
{
    public class LoginFacade
    {
        private readonly ILoginRepositorio _login;

        public LoginFacade(ILoginRepositorio login)
        {
            _login = login;
        }

        public async Task<bool> Login(string username, string password)
        {
            return await _login.Login(username, password);
        }

        public async Task Logout()
        {
            await _login.Logout();
        }
    }
}
