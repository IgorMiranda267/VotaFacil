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
            return await _contexto.Eleitores.FindAsync(id);
        }

        public async Task<IEnumerable<EleitorModel>> ObterTodosEleitores()
        {
            return await _contexto.Eleitores.ToListAsync();
        }

        public async Task AdicionarEleitor(EleitorModel Eleitor)
        {
            _contexto.Eleitores.Add(Eleitor);
            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizarEleitor(EleitorModel Eleitor)
        {
            _contexto.Eleitores.Update(Eleitor);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeletarEleitor(Guid id)
        {
            var Eleitor = await _contexto.Eleitores.FindAsync(id);
            _contexto.Eleitores.Remove(Eleitor);
            await _contexto.SaveChangesAsync();
        }
    }
}
