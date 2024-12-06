namespace VotaFacil.Domain.Interfaces
{
    public interface ITotpService
    {
        string GenerateCode();
        bool VerifyCode(string code);
    }
}
