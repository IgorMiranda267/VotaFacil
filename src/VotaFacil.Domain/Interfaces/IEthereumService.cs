using Nethereum.RPC.Eth.DTOs;

namespace VotaFacil.Domain.Interfaces
{
    public interface IEthereumService
    {
        Task<string> GetLatestBlockAsync();
        Task<string> GetLatestBlockHashAsync();
        Task<string> ConsultarVotoAsync(Guid eleitorId);
        Task<TransactionReceipt> CriarContratoEleicaoAsync(Guid eleicaoId, string nomeEleicao);
        Task<string> EnviarVotoAsync(Guid eleitorId, Guid opcaoVotoId, string hashAnterior, string numeroBloco);

    }
}
