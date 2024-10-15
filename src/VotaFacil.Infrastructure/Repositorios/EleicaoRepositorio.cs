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

        #region ELEICAO
        public async Task<EleicaoModel?> ObterVotacaoPorId(Guid? id)
        {
            return await _contexto.Votacoes
                .Include(e => e.Candidatos)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<EleicaoModel>> ObterTodasEleicoes()
        {
            return await _contexto.Votacoes
                .Include(e => e.Candidatos)
                .ToListAsync();
        }

        public async Task AdicionarEleicao(EleicaoModel eleicao)
        {
            _contexto.Votacoes.Add(eleicao);
            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizarEleicao(EleicaoModel eleicao)
        {
            _contexto.Votacoes.Update(eleicao);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeletarEleicao(Guid id)
        {
            var eleicao = await _contexto.Votacoes.FindAsync(id);
            _contexto.Votacoes.Remove(eleicao);
            await _contexto.SaveChangesAsync();
        }

        public async Task AdicionarCandidatoAEleicao(EleicaoModel eleicao, CandidatoModel candidato)
        {
            var eleicaoExistente = await _contexto.Votacoes
                .Include(e => e.Candidatos)
                .FirstOrDefaultAsync(e => e.Id == eleicao.Id);

            if (eleicaoExistente != null)
            {
                eleicaoExistente.Candidatos.Add(candidato);
                await _contexto.SaveChangesAsync();
            }
        }
        #endregion ELEICAO  

        #region CANDIDATO
        public async Task<bool> AdicionarCandidato(CandidatoModel candidato, EleicaoModel eleicao)
        {
            try
            {
                // Adiciona o candidato ao contexto
                _contexto.Candidatos.Add(candidato);

                // Marca a eleição como inalterada
                _contexto.Entry(eleicao).State = EntityState.Unchanged;

                // Adiciona o candidato à coleção de candidatos da eleição
                eleicao.Candidatos.Add(candidato);

                // Salva as alterações no banco de dados
                var result = await _contexto.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao adicionar candidato: {ex.Message}");
                return false;
            }
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

        public async Task<CandidatoModel> BuscarCandidatoPorId(Guid id)
        {
            return await _contexto.Candidatos.FindAsync(id);
        }

        public async Task<List<CandidatoModel>> BuscarCandidatoPorEleicao(Guid eleicaoId)
        {
            return await _contexto.Candidatos
                .Where(c => c.Votacoes.Any(e => e.Id == eleicaoId))
                .ToListAsync();
        }
        #endregion CANDIDATO
    }
}
