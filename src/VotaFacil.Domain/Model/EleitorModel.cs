using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VotaFacil.Domain.Model;
using VotaFacil.Domain.Validacao;

namespace VotaFacil.Domain.Entidades
{
    [Table("eleitor")]
    public class EleitorModel
    {
        [Key, Column("id")] public Guid Id { get; set; }

        [Required, Column("nome"), MaxLength(100)] public string Nome { get; set; }

        [Required, Column("cpf"), MaxLength(11)] public string Cpf { get; set; }

        [Column("identificador"), MaxLength(50)] public string? Identificador { get; set; }

        [Column("endereco_ethereum"), MaxLength(100)] public string? EnderecoEthereum { get; set; }

        public LoginModel Login { get; set; }
        public ICollection<VotoModel> Votos { get; set; } = new List<VotoModel>();

        public EleitorModel() { }

        public EleitorModel(string nome, string cpf, string? identificador, string? enderecoEthereum, string username, string password)
        {
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(nome), "O nome não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(cpf), "O CPF não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(username), "O username não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(password), "O password não pode ser vazio.");

            Id = Guid.NewGuid();
            Nome = nome;
            Cpf = cpf;
            Identificador = "TESTE";
            EnderecoEthereum = "ENDERECO_TESTE";
            Login = new LoginModel(username, password, Id);
        }

        public bool Autenticar(string identificador, string enderecoEthereum)
        {
            return Identificador == identificador && EnderecoEthereum == enderecoEthereum;
        }
    }
}
