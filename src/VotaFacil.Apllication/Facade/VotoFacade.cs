using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Apllication.Facade
{
    public class VotoFacade
    {
        private readonly IVotoRepositorio _votoRepositorio;

        public VotoFacade(IVotoRepositorio votoRepositorio)
        {
            _votoRepositorio = votoRepositorio;
        }

        public async Task<bool> AdicionarVoto(Guid eleitorId, Guid candidatoId, Guid eleicaoId) ////Guid eleitorId, Guid opcaoVotoId, string hashAnterior
        {
            var voto = new VotoModel(eleitorId, candidatoId, eleicaoId, "HASH_ANTERIOR");
            return await _votoRepositorio.AdicionarVoto(voto);
        }

        public Task<IEnumerable<VotoModel>> ObterVotosPorEleicao(Guid eleicaoId)
        {
            throw new NotImplementedException();
        }

        public async Task<VotoModel?> VerificarVoto(Guid eleicaoId, Guid eleitorId)
        {
            return await _votoRepositorio.VerificarVoto(eleicaoId, eleitorId);
        }
    }
}
