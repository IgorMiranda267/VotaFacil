using VotaFacil.Apllication.DTO;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Apllication.Facade
{
    public class EleitorFacade
    {
        private readonly IEleitorRepositorio _eleitor;

        public EleitorFacade(IEleitorRepositorio eleitorRepositorio)
        {
            _eleitor = eleitorRepositorio;
        }

        public async Task CadastarEleitor(EleitorDTO eleitor)
        {
           // await _eleitor.AdicionarEleitor(eleitor);
        }

        public async Task AtualizarEleitor(EleitorDTO eleitor)
        {
            //await _eleitor.AtualizarEleitor(eleitor); 
        }

        public async Task DeletarEleitor(Guid id)
        {
            await _eleitor.DeletarEleitor(id);
        }

        public async Task<EleitorModel> ObterEleitorPorId(Guid id)
        {
           return await _eleitor.ObterEleitorPorId(id);
        }

        public async Task<IEnumerable<EleitorDTO>> ObterTodosEleitores()
        {
            return null;// await _eleitor.ObterTodosEleitores();
        }
    }
}
