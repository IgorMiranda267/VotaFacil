using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Infrastructure.Service
{
    public class JwtTokenValidator : IJwtTokenValidator
    {
        private readonly string _secretKey;
        private readonly Dictionary<string, DateTime> _revokedTokens;
        private readonly TimeSpan _tokenRevocationDuration;

        public JwtTokenValidator()
        {
        }
        public JwtTokenValidator(string secretKey, TimeSpan tokenRevocationDuration)
        {
            _secretKey = secretKey;
            _revokedTokens = new Dictionary<string, DateTime>();
            _tokenRevocationDuration = tokenRevocationDuration;
        }


        public string GerarToken(LoginModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.EleitorId.ToString()), // Armazena o ID do usuário
                    new Claim(ClaimTypes.Name, user.Username), // Armazena o nome de usuário
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidarToken(string token)
        {
            if (RevogarToken(token))
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireSignedTokens = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal != null;
            }
            catch
            {
                return false;
            }
        }

        public void InvalidarToken(string token)
        {
            _revokedTokens[token] = DateTime.UtcNow;
        }

        private bool RevogarToken(string token)
        {
            if (_revokedTokens.TryGetValue(token, out DateTime revocationTime))
            {
                if (DateTime.UtcNow - revocationTime < _tokenRevocationDuration)
                    return true;

                else
                    _revokedTokens.Remove(token); // Remove tokens expirados
            }
            return false;
        }

        public Guid? ObterEleitorIdDoToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireSignedTokens = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var eleitorIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (eleitorIdClaim != null && Guid.TryParse(eleitorIdClaim.Value, out var eleitorId))
                {
                    return eleitorId;
                }
            }
            catch
            {
                throw new Exception("Token inválido.");
            }
            return null;
        }
    }
}
