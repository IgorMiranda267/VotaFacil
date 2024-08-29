using VotaFacil.Domain.Entidades;

namespace VotaFacil.Domain.Interfaces
{
    public interface IVotacaoRepositorio
    {
        Task<VotacaoModel> ObterVotacaoPorId(Guid id);
        Task<IEnumerable<VotacaoModel>> ObterTodasVotacoes();
        Task AdicionarVotacao(VotacaoModel votacao);
        Task AtualizarVotacao(VotacaoModel votacao);
    }
}
