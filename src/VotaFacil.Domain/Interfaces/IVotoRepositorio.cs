using VotaFacil.Domain.Entidades;

namespace VotaFacil.Domain.Interfaces
{
    public interface IVotoRepositorio
    {
        Task<bool> AdicionarVoto(VotoModel voto);
        Task<IEnumerable<VotoModel>> ObterVotosPorEleicao(Guid eleicaoId);
        Task<VotoModel?> VerificarVoto(Guid eleicaoId, Guid eleitorId);
    }
}
