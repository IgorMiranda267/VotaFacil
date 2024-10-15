using Microsoft.AspNetCore.Http;
using VotaFacil.Domain.Entidades;

namespace VotaFacil.Apllication.DTO
{
    public class CandidatoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public IFormFile Foto { get; set; }
        public string? FotoPath { get; set; }
        public List<VotoModel> Votos { get; set; }
    }
}
