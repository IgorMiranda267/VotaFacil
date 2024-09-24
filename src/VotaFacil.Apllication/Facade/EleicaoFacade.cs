using AutoMapper;
using VotaFacil.Apllication.DTO;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Apllication.Facade
{
    public class EleicaoFacade
    {
        IMapper _mapper;
        IEleicaoRepositorio _eleicaoRepositorio;
        public EleicaoFacade(IMapper mapper, IEleicaoRepositorio eleicaoRepositorio) 
        { 
            _mapper = mapper;
            _eleicaoRepositorio = eleicaoRepositorio;
        }

        #region ELEIÇÂO
        public async Task CadastrarEleicao(EleicaoDTO eleicao)
        {
            var eleicaoModel = new EleicaoModel(
                eleicao.Nome,
                eleicao.Descricao,
                eleicao.Inicio,
                eleicao.Fim
            );

            await _eleicaoRepositorio.AdicionarEleicao(eleicaoModel);
        }
        #endregion
    }
}
