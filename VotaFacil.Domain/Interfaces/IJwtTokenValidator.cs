using System.Security.Claims;

namespace VotaFacil.Domain.Interfaces
{
    public interface IJwtTokenValidator
    {
        string GerarToken(string Name, string Username, string secretKey, int expiracaoHoras = 1);
        ClaimsPrincipal? ValidarToken(string token);
        void InvalidarToken(string token);
    }
}
