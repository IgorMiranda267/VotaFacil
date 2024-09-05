using VotaFacil.Domain.Entidades;

namespace VotaFacil.Domain.Interfaces
{
    public interface IEleitorRepositorio
    {
        Task<EleitorModel> ObterEleitorPorId(Guid id);
        Task<IEnumerable<EleitorModel>> ObterTodosEleitores();
        Task AdicionarEleitor(EleitorModel Eleitor);
        Task AtualizarEleitor(EleitorModel Eleitor);
        Task DeletarEleitor(Guid id);
    }
}
