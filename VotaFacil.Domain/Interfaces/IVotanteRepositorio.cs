using VotaFacil.Domain.Entidades;

namespace VotaFacil.Domain.Interfaces
{
    public interface IVotanteRepositorio
    {
        Task<VotanteModel> ObterVotantePorId(Guid id);
        Task<IEnumerable<VotanteModel>> ObterTodosVotantes();
        Task AdicionarVotante(VotanteModel votante);
        Task AtualizarVotante(VotanteModel votante);
        Task DeletarVotante(Guid id);
    }
}
