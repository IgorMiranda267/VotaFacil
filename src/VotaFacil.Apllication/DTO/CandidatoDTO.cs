using Microsoft.AspNetCore.Http;

namespace VotaFacil.Apllication.DTO
{
    public class CandidatoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public IFormFile Foto { get; set; }
        public string? FotoPath { get; set; }
        public int Votos { get; set; }
    }
}
