using AutoMapper;
using VotaFacil.Apllication.DTO;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Domain.Model;

namespace VotaFacil.Apllication.Facade
{
    public class VotacaoFacade
    {
        private readonly IVotacaoRepositorio _votacaoRepositorio;
        private IMapper _mapper;

        public VotacaoFacade(IVotacaoRepositorio votacaoRepositorio, IMapper mapper)
        {
            _votacaoRepositorio = votacaoRepositorio;
            _mapper = mapper;
        }

        //public async Task<VotacaoModel> ObterVotacaoPorId(Guid id)
        //{

        //}
        
        //public async Task<IEnumerable<VotacaoModel>> ObterTodasVotacoes()
        //{

        //}
        
        public async Task AdicionarVotacao(VotacaoModel votacao)
        {

        }
        
        public async Task AtualizarVotacao(VotacaoModel votacao)
        {

        }
        
        public async Task<bool> AdicionarCandidato(CandidatoDTO candidato)
        {
            var cad = new CandidatoModel(candidato.Nome, candidato.Descricao, candidato.Foto);
            return await _votacaoRepositorio.AdicionarCandidato(cad);
        }
        
        public async Task<bool> AtualizarCandidato(CandidatoDTO candidato)
        {
            var candidatoMap = _mapper.Map<CandidatoModel>(candidato);
            return await _votacaoRepositorio.AtualizarCandidato(candidatoMap);
        }
        
        public async Task DeletarCandidato(Guid idCandidato)
        {

        }

        public async Task<List<CandidatoDTO>> BuscarTodosCandidato()
        {
            var candidatosList = await _votacaoRepositorio.BuscarTodosCandidato();
          
            return  _mapper.Map<List<CandidatoDTO>>(candidatosList);
        }
    }
}
