using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VotaFacil.Domain.Validacao;
using Microsoft.AspNetCore.Http;

namespace VotaFacil.Domain.Model
{
    [Table("candidato")]
    public class CandidatoModel
    {
        [Key, Column("id")] public Guid Id { get; set; }

        [Required, Column("nome"), MaxLength(100)] public string Nome { get; set; }

        [Required, Column("descricao"), MaxLength(500)] public string Descricao { get; set; }

        [Required, Column("foto")] public string FotoPath { get; set; }

        [NotMapped] public IFormFile Foto { get; set; }

        public CandidatoModel() { }

        public CandidatoModel(string nome, string descricao, IFormFile foto)
        {
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(nome), "O nome não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(descricao), "A descrição não pode ser vazia.");
            ValidacaoDeExcecaoDominio.When(foto == null, "A foto não pode ser nula.");

            Id = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            Foto = foto;
            FotoPath = $"images/{foto.FileName}";
        }
    }
}
