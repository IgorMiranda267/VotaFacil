using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infra.Data.Contexto;

namespace VotaFacil.Infrastructure.Repositorios
{
    public class LoginRepositorio : ILoginRepositorio
    {
        private readonly VotacaoContext _contexto;
        private readonly IJwtTokenService _jwtTokenValidator;

        public LoginRepositorio(VotacaoContext contexto, IJwtTokenService jwtTokenValidator)
        {
            _contexto = contexto;
            _jwtTokenValidator = jwtTokenValidator;
        }

        public async Task<(bool, string)> Login(string username, string password)
        {
            var user = await _contexto.Set<LoginModel>()
                                      .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                if (_jwtTokenValidator.ValidarToken(user.Token))
                    return (true, user.Token);

                var token = _jwtTokenValidator.GerarToken(user);
                user.Token = token;
                user.UltimoLogin = DateTime.UtcNow;
                user.ExpiracaoToken = DateTime.UtcNow.AddHours(1); // Defina a expiração do token conforme necessário

                _contexto.Logins.Update(user);
                await _contexto.SaveChangesAsync();

                return (true, token);
            }
            return (false, null);
        }

        public async Task Logout(string token)
        {
            if (_jwtTokenValidator.ValidarToken(token))
            {
                _jwtTokenValidator.InvalidarToken(token);
            }
            await Task.CompletedTask;
        }
    }
}
