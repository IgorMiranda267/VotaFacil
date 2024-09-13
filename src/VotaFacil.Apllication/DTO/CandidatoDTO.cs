using Microsoft.AspNetCore.Http;

namespace VotaFacil.Apllication.DTO
{
    public class CandidatoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public IFormFile Foto { get; set; }
    }
}
