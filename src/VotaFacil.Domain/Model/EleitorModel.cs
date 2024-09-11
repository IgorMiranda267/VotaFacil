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

        [Column("identificador"), MaxLength(50)] public string Identificador { get; set; }

        [Column("endereco_ethereum"), MaxLength(100)] public string EnderecoEthereum { get; set; }

        [Column("conta_ethereum_id"), ForeignKey("ContaEthereumModel")] public Guid ContaEthereumId { get; set; }
        public ContaEthereumModel ContaEthereum { get; set; }

        public List<VotacaoModel> Votacoes { get; set; } = new List<VotacaoModel>();

        public EleitorModel(string nome, string cpf)
        {
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(nome), "O nome não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(cpf), "O nome não pode ser vazio.");


            Id = Guid.NewGuid();
            Nome = nome;
            Cpf = cpf;
        }

        public bool Autenticar(string identificador, string enderecoEthereum)
        {
            return Identificador == identificador && EnderecoEthereum == enderecoEthereum;
        }
    }
}
