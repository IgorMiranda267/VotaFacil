using VotaFacil.Domain.Validacao;

namespace VotaFacil.Domain.Entidades
{
    public class VotanteModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Identificador { get; set; }
        public string EnderecoEthereum { get; set; }

        public VotanteModel(Guid id, string nome, string cpf, string identificador, string enderecoEthereum)
        {
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(nome), "O nome não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(cpf), "O nome não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(identificador), "O nome identificador pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(enderecoEthereum), "O endereco Ethereum não pode ser vazio.");

            Id = id;
            Nome = nome;
            Cpf = cpf;
            Identificador = identificador;
            EnderecoEthereum = enderecoEthereum;
        }

        public bool Autenticar(string identificador, string enderecoEthereum)
        {
            return Identificador == identificador && EnderecoEthereum == enderecoEthereum;
        }
    }
}
