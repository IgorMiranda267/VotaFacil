using AutoMapper;
using VotaFacil.Apllication.DTO;
using VotaFacil.Domain.Entidades;

namespace VotaFacil.Apllication.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EleitorDTO, EleitorModel>().ReverseMap();
        }
    }
}
