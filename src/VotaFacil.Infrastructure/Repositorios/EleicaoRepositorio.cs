using Microsoft.EntityFrameworkCore;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Domain.Model;
using VotaFacil.Infra.Data.Contexto;

namespace VotaFacil.Infra.Data.Repositorios
{
    public class EleicaoRepositorio : IEleicaoRepositorio
    {
        private readonly VotacaoContext _contexto;

        public EleicaoRepositorio(VotacaoContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<EleicaoModel> ObterVotacaoPorId(Guid id)
        {
            return await _contexto.Votacoes.FindAsync(id);
        }

        public async Task<IEnumerable<EleicaoModel>> ObterTodasEleicoes()
        {
            return await _contexto.Votacoes.ToListAsync();
        }

        public async Task AdicionarEleicao(EleicaoModel votacao)
        {
            _contexto.Votacoes.Add(votacao);
            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizarEleicao(EleicaoModel votacao)
        {
            _contexto.Votacoes.Update(votacao);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeletarEleicao(Guid id)
        {
            var votacao = await _contexto.Votacoes.FindAsync(id);
            _contexto.Votacoes.Remove(votacao);
            await _contexto.SaveChangesAsync();
        }

        public async Task<bool> AdicionarCandidato(CandidatoModel candidato)
        {
            _contexto.Candidatos.Add(candidato);
            var result = await _contexto.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> AtualizarCandidato(CandidatoModel candidato)
        {
            _contexto.Candidatos.Update(candidato);
            var result = await _contexto.SaveChangesAsync();
            return result > 0;
        }

        public async Task DeletarCandidato(Guid idCandidato)
        {
            var cadidato = await _contexto.Candidatos.FindAsync(idCandidato);
            _contexto.Candidatos.Remove(cadidato);
            await _contexto.SaveChangesAsync();
        }

        public async Task<List<CandidatoModel>> BuscarTodosCandidato()
        {
            return await _contexto.Candidatos.ToListAsync();
        }
    }
}
