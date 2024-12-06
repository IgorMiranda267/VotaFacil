using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infra.Data.Contexto;
using Microsoft.EntityFrameworkCore;

namespace VotaFacil.Infrastructure.Repositorios
{
    public class VotoRepositorio : IVotoRepositorio
    {
        private readonly VotacaoContext _contexto;

        public VotoRepositorio(VotacaoContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> AdicionarVoto(VotoModel voto)
        {
            try
            {
                _contexto.Votos.Add(voto);
                var result = await _contexto.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao adicionar voto: {ex.Message}");
                return false;
            }
        }

        public async Task<VotoModel?> VerificarVoto(Guid eleicaoId, Guid eleitorId)
        {
            return await _contexto.Votos
                .Include(v => v.Candidato)
                .Include(v => v.Eleitor)
                .Include(v => v.Votacao)
                .FirstOrDefaultAsync(v => v.VotacaoId == eleicaoId && v.EleitorId == eleitorId);
        }

        public async Task<IEnumerable<VotoModel>> ObterVotosPorEleicao(Guid eleicaoId)
        {
            return await _contexto.Votos
                .Where(v => v.VotacaoId == eleicaoId)
                .Include(v => v.Candidato)
                .Include(v => v.Eleitor)
                .Include(v => v.Votacao)
                .ToListAsync();
        }
    }
}
