using OtpNet;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Infrastructure.Service
{
    public class TotpService : ITotpService
    {
        private readonly byte[] _secretKey;

        public TotpService(string secretKey)
        {
            _secretKey = Base32Encoding.ToBytes(secretKey);
        }

        public string GenerateCode()
        {
            var totp = new Totp(_secretKey);
            return totp.ComputeTotp();
        }

        public bool VerifyCode(string code)
        {
            var totp = new Totp(_secretKey);
            return totp.VerifyTotp(code, out long timeStepMatched, new VerificationWindow(2, 2));
        }
    }
}