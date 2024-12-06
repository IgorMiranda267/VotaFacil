using VotaFacil.Domain.Interfaces;
using VotaFacil.Infrastructure.Service;

namespace VotaFacil.Apllication.Controller
{
    public class LoginFacade
    {
        private readonly ILoginRepositorio _loginRepositorio;
        private readonly IEmailService _emailService;
        private readonly ITotpService _totpService;

        public LoginFacade(ILoginRepositorio login, IEmailService emailService, ITotpService totpService)
        {
            _loginRepositorio = login;
            _emailService = emailService;
            _totpService = totpService;
        }

        public async Task<(bool, string)> Login(string username, string password)
        {
            return await _loginRepositorio.Login(username, password);
        }

        public async Task Logout(string token)
        {
            await _loginRepositorio.Logout(token);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await EmailService.SendEmailAsyncs(to, subject, body);
            //await _emailService.SendEmailAsync(to, subject, body);
        }

        public string GenerateCode()
        {
            return _totpService.GenerateCode();
        }

        public bool VerifyCode(string code)
        {
            return _totpService.VerifyCode(code);
        }
    }
}
