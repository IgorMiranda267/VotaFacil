using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Model;

namespace VotaFacil.Domain.Interfaces
{
    public interface IVotacaoRepositorio
    {
        Task<VotacaoModel> ObterVotacaoPorId(Guid id);
        Task<IEnumerable<VotacaoModel>> ObterTodasVotacoes();
        Task AdicionarVotacao(VotacaoModel votacao);
        Task AtualizarVotacao(VotacaoModel votacao);
        Task<bool> AdicionarCandidato(CandidatoModel candidato);
        Task<bool> AtualizarCandidato(CandidatoModel candidato);
        Task DeletarCandidato(Guid idCandidato);
        Task<List<CandidatoModel>> BuscarTodosCandidato();
    }
}
