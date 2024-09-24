using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Model;

namespace VotaFacil.Domain.Interfaces
{
    public interface IEleicaoRepositorio
    {
        Task<EleicaoModel> ObterVotacaoPorId(Guid id);
        Task<IEnumerable<EleicaoModel>> ObterTodasEleicoes();
        Task AdicionarEleicao(EleicaoModel votacao);
        Task AtualizarEleicao(EleicaoModel votacao);
        Task<bool> AdicionarCandidato(CandidatoModel candidato);
        Task<bool> AtualizarCandidato(CandidatoModel candidato);
        Task DeletarCandidato(Guid idCandidato);
        Task<List<CandidatoModel>> BuscarTodosCandidato();
    }
}
