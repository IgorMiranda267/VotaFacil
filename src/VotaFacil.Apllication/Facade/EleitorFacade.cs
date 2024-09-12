using AutoMapper;
using VotaFacil.Apllication.DTO;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Apllication.Facade
{
    public class EleitorFacade
    {
        private readonly IEleitorRepositorio _eleitor;
        private readonly IMapper _mapper;

        public EleitorFacade(IEleitorRepositorio eleitorRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _eleitor = eleitorRepositorio;
        }

        public async Task CadastarEleitor(EleitorDTO eleitor)
        {
            var eleitorModel = new EleitorModel(
                eleitor.Nome,
                eleitor.CPF,
                eleitor.Identificador,
                eleitor.EnderecoEthereum,
                eleitor.Username,
                eleitor.Password
            );
            await _eleitor.AdicionarEleitor(eleitorModel);
        }

        public async Task AtualizarEleitor(EleitorDTO eleitor)
        {
            var eleitorMap = _mapper.Map<EleitorModel>(eleitor);
            await _eleitor.AtualizarEleitor(eleitorMap); 
        }

        public async Task DeletarEleitor(Guid id)
        {
            await _eleitor.DeletarEleitor(id);
        }

        public async Task<EleitorModel> ObterEleitorPorId(Guid id)
        {
           return await _eleitor.ObterEleitorPorId(id);
        }

        public async Task<IEnumerable<EleitorDTO>> ObterTodosEleitores()
        {
            IEnumerable<EleitorModel> eleitores = await _eleitor.ObterTodosEleitores();
            return _mapper.Map<IEnumerable<EleitorDTO>>(eleitores);
        }
    }
}
