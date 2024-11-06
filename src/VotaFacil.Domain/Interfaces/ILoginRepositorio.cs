namespace VotaFacil.Domain.Interfaces
{
    public interface ILoginRepositorio
    {
        Task<(bool, string)> Login(string username, string password);
        Task Logout(string token);

    }
}
