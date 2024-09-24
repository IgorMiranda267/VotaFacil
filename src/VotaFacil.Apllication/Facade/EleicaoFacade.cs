using AutoMapper;
using VotaFacil.Apllication.DTO;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Domain.Model;

namespace VotaFacil.Apllication.Facade
{
    public class EleicaoFacade
    {
        private readonly IEleicaoRepositorio _eleicaoRepositorio;
        private IMapper _mapper;

        public EleicaoFacade(IEleicaoRepositorio eleicaoRepositorio, IMapper mapper)
        {
            _eleicaoRepositorio = eleicaoRepositorio;
            _mapper = mapper;
        }

        
        public async Task<bool> AdicionarCandidato(CandidatoDTO candidato)
        {
            var cad = new CandidatoModel(candidato.Nome, candidato.Descricao, candidato.Foto);
            return await _eleicaoRepositorio.AdicionarCandidato(cad);
        }
        
        public async Task<bool> AtualizarCandidato(CandidatoDTO candidato)
        {
            var candidatoMap = _mapper.Map<CandidatoModel>(candidato);
            return await _eleicaoRepositorio.AtualizarCandidato(candidatoMap);
        }
        
        public async Task DeletarCandidato(Guid idCandidato)
        {

        }

        public async Task<List<CandidatoDTO>> BuscarTodosCandidato()
        {
            var candidatosList = await _eleicaoRepositorio.BuscarTodosCandidato();
          
            return  _mapper.Map<List<CandidatoDTO>>(candidatosList);
        }

        public async Task CadastrarEleicao(EleicaoDTO eleicao)
        {
            var eleicaoModel = new EleicaoModel(
                eleicao.Nome,
                eleicao.Descricao,
                DateTime.SpecifyKind(eleicao.Inicio, DateTimeKind.Utc),
                DateTime.SpecifyKind(eleicao.Fim, DateTimeKind.Utc)
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
    }
}
