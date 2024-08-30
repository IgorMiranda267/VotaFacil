using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infra.Data.Contexto;

namespace VotaFacil.Infra.Data.Repositorios
{
    public class EleitorRepositorio : IEleitorRepositorio
    {
        private readonly VotacaoContext _contexto;

        public EleitorRepositorio(VotacaoContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<EleitorModel> ObterEleitorPorId(Guid id)
        {
            return await _contexto.Eleitors.FindAsync(id);
        }

        public async Task<IEnumerable<EleitorModel>> ObterTodosEleitores()
        {
            return await _contexto.Eleitors.ToListAsync();
        }

        public async Task AdicionarEleitor(EleitorModel Eleitor)
        {
            _contexto.Eleitors.Add(Eleitor);
            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizarEleitor(EleitorModel Eleitor)
        {
            _contexto.Eleitors.Update(Eleitor);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeletarEleitor(Guid id)
        {
            var Eleitor = await _contexto.Eleitors.FindAsync(id);
            _contexto.Eleitors.Remove(Eleitor);
            await _contexto.SaveChangesAsync();
        }
    }
}
