using System.Security.Claims;
using VotaFacil.Domain.Entidades;

namespace VotaFacil.Domain.Interfaces
{
    public interface IJwtTokenValidator
    {
        string GerarToken(LoginModel user);
        bool ValidarToken(string token);
        void InvalidarToken(string token);
        Guid? ObterEleitorIdDoToken(string token);
    }
}
