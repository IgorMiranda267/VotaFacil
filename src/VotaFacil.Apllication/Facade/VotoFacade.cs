using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Apllication.Facade
{
    public class VotoFacade
    {
        private readonly IEleitorRepositorio _eleitorRepositorio;
        private readonly IVotoRepositorio _votoRepositorio;
        private readonly IEthereumService _ethereumService;
        private readonly IEleicaoRepositorio _eleicaoRepositorio;

        public VotoFacade(IVotoRepositorio votoRepositorio, IEthereumService ethereumService, IEleicaoRepositorio eleicaoRepositorio, IEleitorRepositorio eleitorRepositorio)
        {
            _votoRepositorio = votoRepositorio;
            _ethereumService = ethereumService;
            _eleicaoRepositorio = eleicaoRepositorio;
            _eleitorRepositorio = eleitorRepositorio;
        }

        public async Task<bool> AdicionarVoto(Guid eleitorId, Guid candidatoId, Guid eleicaoId) ////Guid eleitorId, Guid opcaoVotoId, string hashAnterior
        {
            var numeroBloco = await _ethereumService.GetLatestBlockAsync();
            var hashAnterior = await _ethereumService.GetLatestBlockHashAsync();

            var eleicao = await _eleicaoRepositorio.ObterVotacaoPorId(eleicaoId);
            var eleitor = await _eleitorRepositorio.ObterEleitorPorId(eleitorId);

            var txHashs = await _ethereumService.EnviarVotoAsync(eleicao.ContractAddress, eleitorId, candidatoId, hashAnterior, numeroBloco, eleitor.ChavePrivada);
            var txHash = await _ethereumService.EnviarVotoAsync(eleicao.ContractAddress, eleitorId, candidatoId, hashAnterior, numeroBloco);

            var voto = new VotoModel(eleitorId, candidatoId, eleicaoId, hashAnterior, numeroBloco, txHashs.SignedTransaction);
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
