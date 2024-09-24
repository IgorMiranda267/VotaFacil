using System.ComponentModel.DataAnnotations;

namespace VotaFacil.Apllication.DTO
{
    public class EleicaoDTO
    {
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; }

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fim { get; set; }
    }
}
