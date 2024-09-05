using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Infrastructure.Service
{
    public class JwtTokenValidator : IJwtTokenValidator
    {
        private readonly string _secretKey;
        private readonly Dictionary<string, DateTime> _revokedTokens;
        private readonly TimeSpan _tokenRevocationDuration;

        public JwtTokenValidator(string secretKey, TimeSpan tokenRevocationDuration)
        {
            _secretKey = secretKey;
            _revokedTokens = new Dictionary<string, DateTime>();
            _tokenRevocationDuration = tokenRevocationDuration; ;
        }

        public string GerarToken(string Name, string Username, string secretKey, int expiracaoHoras = 1)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Username)
                }),
                Expires = DateTime.UtcNow.AddHours(expiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidarToken(string token)
        {
            if (RevogarToken(token))
                return null; 

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

                return principal;
            }
            catch
            {
                return null;
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
    }
}
