using VotaFacil.Domain.Entidades;

namespace VotaFacil.Domain.Interfaces
{
    public interface IVotanteRepositorio
    {
        Task<EleitorModel> ObterVotantePorId(Guid id);
        Task<IEnumerable<EleitorModel>> ObterTodosVotantes();
        Task AdicionarVotante(EleitorModel votante);
        Task AtualizarVotante(EleitorModel votante);
        Task DeletarVotante(Guid id);
    }
}
