using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VotaFacil.Domain.Entidades
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? UltimoLogin { get; set; } 
        public bool Status { get; set; } 
        public DateTime? ExpiracaoToken { get; set; }

        public LoginModel()
        {
            CriadoEm = DateTime.UtcNow;
            Status = true;
        }

        public async Task<bool> Login()
        {
            if (!Status)
             return false;

            UltimoLogin = DateTime.UtcNow;
            ExpiracaoToken = DateTime.UtcNow.AddHours(1);
            return true;
        }

        public async Task Logout()
        {
            UltimoLogin = DateTime.UtcNow;
        }
    }
}
