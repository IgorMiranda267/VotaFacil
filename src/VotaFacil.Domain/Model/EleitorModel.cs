using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using VotaFacil.Domain.Validacao;

namespace VotaFacil.Domain.Entidades
{
    [Table("eleitor")]
    public class EleitorModel
    {
        [Key, Column("id")] public Guid Id { get; set; }

        [Required, Column("nome"), MaxLength(100)] public string Nome { get; set; }

        [Required, Column("email"), MaxLength(100)] public string Email { get; set; }

        [Required, Column("cpf"), MaxLength(11)] public string Cpf { get; set; }

        [Column("identificador"), MaxLength(50)] public string? Identificador { get; set; }

        [Column("endereco_ethereum"), MaxLength(100)] public string? EnderecoEthereum { get; set; }

        [Column("chave_privada"), MaxLength(4096)] public string ChavePrivada { get; set; }

        [Column("chave_publica"), MaxLength(4096)] public string ChavePublica { get; set; }
        [Column("codigo_verificacao"), MaxLength(6)] public string? CodigoVerificacao { get; set; }

        public LoginModel Login { get; set; }
        public ICollection<VotoModel> Votos { get; set; } = new List<VotoModel>();

        public EleitorModel() { }

        public EleitorModel(string nome, string email, string cpf, string? identificador, string? enderecoEthereum, string username, string password)
        {
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(nome), "O nome não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(cpf), "O CPF não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(username), "O username não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(password), "O password não pode ser vazio.");

            Id = Guid.NewGuid();
            Nome = nome;
            Email = Email;
            Cpf = cpf;
            Identificador = identificador;
            EnderecoEthereum = enderecoEthereum;
            Login = new LoginModel(username, password, Id);

            // Gerar chaves pública e privada no formato hexadecimal
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                var privateKeyBytes = rsa.ExportRSAPrivateKey();
                var publicKeyBytes = rsa.ExportRSAPublicKey();

                ChavePrivada = BitConverter.ToString(privateKeyBytes).Replace("-", "").ToLower();
                ChavePublica = BitConverter.ToString(publicKeyBytes).Replace("-", "").ToLower();
            }
        }

        public bool Autenticar(string identificador, string enderecoEthereum)
        {
            return Identificador == identificador && EnderecoEthereum == enderecoEthereum;
        }
    }
}
