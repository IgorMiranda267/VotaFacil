using Nethereum.RPC.Eth.DTOs;
using VotaFacil.Domain.DTO;

namespace VotaFacil.Domain.Interfaces
{
    public interface IEthereumService
    {
        Task<string> GetLatestBlockAsync();
        Task<string> GetLatestBlockHashAsync();
        Task<string> ConsultarVotoAsync(string transactionHash);
        Task<List<VotoRegistradoEventDTO>> GetVotosRegistradosAsync(string transactionHash);
        Task<TransactionReceipt> CriarContratoEleicaoAsync(Guid eleicaoId, string nomeEleicao);
        Task<string> EnviarVotoAsync(string enderecoContrato, Guid eleitorId, Guid opcaoVotoId, string hashAnterior, string numeroBloco);
        Task<(string TransactionHash, string SignedTransaction)> EnviarVotoAsync(string enderecoContrato, Guid eleitorId, Guid opcaoVotoId, string hashAnterior, string numeroBloco, string chavePrivadaEleitor);

    }
}
