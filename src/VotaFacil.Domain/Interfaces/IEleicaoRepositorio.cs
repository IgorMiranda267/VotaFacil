using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Model;

namespace VotaFacil.Domain.Interfaces
{
    public interface IEleicaoRepositorio
    {
        Task<bool> AtualizarCandidato(CandidatoModel candidato);
        Task<bool> AdicionarCandidato(CandidatoModel candidato, EleicaoModel eleicao);
        Task DeletarCandidato(Guid idCandidato);
        Task AtualizarEleicao(EleicaoModel votacao);
        Task AdicionarEleicao(EleicaoModel votacao);
        Task<EleicaoModel?> ObterVotacaoPorId(Guid? id);
        Task<List<CandidatoModel>> BuscarTodosCandidato();
        Task<CandidatoModel> BuscarCandidatoPorId(Guid id);
        Task<IEnumerable<EleicaoModel>> ObterTodasEleicoes();
        Task<List<CandidatoModel>> BuscarCandidatoPorEleicao(Guid eleicaoId);
    }
}
