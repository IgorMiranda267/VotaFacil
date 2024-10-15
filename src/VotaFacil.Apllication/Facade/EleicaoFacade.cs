using AutoMapper;
using VotaFacil.Apllication.DTO;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Domain.Model;

namespace VotaFacil.Apllication.Facade
{
    public class EleicaoFacade
    {
        private IMapper _mapper;
        private readonly IEthereumService _ethereumService;
        private readonly IEleicaoRepositorio _eleicaoRepositorio;

        public EleicaoFacade(IEleicaoRepositorio eleicaoRepositorio, IMapper mapper, IEthereumService ethereumService)
        {
            _mapper = mapper;
            _ethereumService = ethereumService;
            _eleicaoRepositorio = eleicaoRepositorio;
        }

        #region CANDIDATO
        public async Task<bool> AdicionarCandidato(CandidatoDTO candidato, EleicaoModel eleicao)
        {
            var cad = new CandidatoModel(candidato.Nome, candidato.Descricao, candidato?.FotoPath);
            return await _eleicaoRepositorio.AdicionarCandidato(cad, eleicao);
        }
        
        public async Task<bool> AtualizarCandidato(CandidatoDTO candidato)
        {
            var candidatoMap = _mapper.Map<CandidatoModel>(candidato);
            return await _eleicaoRepositorio.AtualizarCandidato(candidatoMap);
        }

        public async Task<CandidatoModel> BuscarCandidatoPorId(Guid id)
        {
            return await _eleicaoRepositorio.BuscarCandidatoPorId(id);
        }

        public async Task DeletarCandidato(Guid idCandidato)
        {

        }

        public async Task<List<CandidatoDTO>> BuscarTodosCandidato()
        {
            var candidatosList = await _eleicaoRepositorio.BuscarTodosCandidato();
          
            return  _mapper.Map<List<CandidatoDTO>>(candidatosList);
        }

        public async Task<List<CandidatoModel>> BuscarCandidatoPorEleicao(Guid eleicaoId)
        {
            return await _eleicaoRepositorio.BuscarCandidatoPorEleicao(eleicaoId);
        }
        #endregion CANDIDATO

        #region ELEIÇÂO
        public async Task CadastrarEleicao(EleicaoDTO eleicao)
        {
            var contrato = await _ethereumService.CriarContratoEleicaoAsync(eleicao.Id, eleicao.Nome);

            var eleicaoModel = new EleicaoModel(
                eleicao.Nome,
                eleicao.Descricao,
                DateTime.SpecifyKind(eleicao.Inicio, DateTimeKind.Utc),
                DateTime.SpecifyKind(eleicao.Fim, DateTimeKind.Utc),
                contrato.ContractAddress,
                contrato.TransactionHash,
                contrato.BlockNumber,
                contrato.GasUsed
            );

            await _eleicaoRepositorio.AdicionarEleicao(eleicaoModel);
        }

        public async Task<EleicaoModel> ObterVotacaoPorId(Guid id)
        {
            return await _eleicaoRepositorio.ObterVotacaoPorId(id);
        }

        public async Task<IEnumerable<EleicaoModel>> ObterTodasEleicoes()
        {
            return await _eleicaoRepositorio.ObterTodasEleicoes();
        }

        public async Task AdicionarEleicao(EleicaoModel votacao)
        {
            await _eleicaoRepositorio.AdicionarEleicao(votacao);
        }

        public async Task AtualizarEleicao(EleicaoModel votacao)
        {
            await _eleicaoRepositorio.AtualizarEleicao(votacao);
        }
        #endregion ELEIÇÂO
    }
}
