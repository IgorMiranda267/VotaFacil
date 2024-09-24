using AutoMapper;
using VotaFacil.Apllication.DTO;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Model;

namespace VotaFacil.Apllication.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EleitorDTO, EleitorModel>().ReverseMap();
            CreateMap<CandidatoDTO, CandidatoModel>().ReverseMap();
            CreateMap<EleicaoModel, EleicaoDTO>().ReverseMap();
        }
    }
}
