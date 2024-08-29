using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infra.Data.Contexto;

namespace VotaFacil.Infra.Data.Repositorios
{
    public class VotanteRepositorio : IVotanteRepositorio
    {
        private readonly VotacaoContext _contexto;

        public VotanteRepositorio(VotacaoContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<VotanteModel> ObterVotantePorId(Guid id)
        {
            return await _contexto.Votantes.FindAsync(id);
        }

        public async Task<IEnumerable<VotanteModel>> ObterTodosVotantes()
        {
            return await _contexto.Votantes.ToListAsync();
        }

        public async Task AdicionarVotante(VotanteModel votante)
        {
            _contexto.Votantes.Add(votante);
            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizarVotante(VotanteModel votante)
        {
            _contexto.Votantes.Update(votante);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeletarVotante(Guid id)
        {
            var votante = await _contexto.Votantes.FindAsync(id);
            _contexto.Votantes.Remove(votante);
            await _contexto.SaveChangesAsync();
        }
    }
}
