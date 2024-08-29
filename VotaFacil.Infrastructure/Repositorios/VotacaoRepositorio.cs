using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infra.Data.Contexto;

namespace VotaFacil.Infra.Data.Repositorios
{
    public class VotacaoRepositorio : IVotacaoRepositorio
    {
        private readonly VotacaoContext _contexto;

        public VotacaoRepositorio(VotacaoContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<VotacaoModel> ObterVotacaoPorId(Guid id)
        {
            return await _contexto.Votacoes.FindAsync(id);
        }

        public async Task<IEnumerable<VotacaoModel>> ObterTodasVotacoes()
        {
            return await _contexto.Votacoes.ToListAsync();
        }

        public async Task AdicionarVotacao(VotacaoModel votacao)
        {
            _contexto.Votacoes.Add(votacao);
            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizarVotacao(VotacaoModel votacao)
        {
            _contexto.Votacoes.Update(votacao);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeletarVotacao(Guid id)
        {
            var votacao = await _contexto.Votacoes.FindAsync(id);
            _contexto.Votacoes.Remove(votacao);
            await _contexto.SaveChangesAsync();
        }
    }
}
