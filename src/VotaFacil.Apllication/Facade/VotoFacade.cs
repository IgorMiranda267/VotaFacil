using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Apllication.Facade
{
    public class VotoFacade
    {
        private readonly IVotoRepositorio _votoRepositorio;
        private readonly IEthereumService _ethereumService;

        public VotoFacade(IVotoRepositorio votoRepositorio)
        {
            _votoRepositorio = votoRepositorio;
        }

        public async Task<bool> AdicionarVoto(Guid eleitorId, Guid candidatoId, Guid eleicaoId) ////Guid eleitorId, Guid opcaoVotoId, string hashAnterior
        {
            var numeroBloco = await _ethereumService.GetLatestBlockAsync();
            var hashAnterior = await _ethereumService.GetLatestBlockHashAsync();

            var voto = new VotoModel(eleitorId, candidatoId, eleicaoId, hashAnterior, numeroBloco);
            var txHash = await _ethereumService.EnviarVotoAsync(eleitorId, candidatoId, hashAnterior, numeroBloco);

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
