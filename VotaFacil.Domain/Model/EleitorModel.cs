using VotaFacil.Domain.Validacao;

namespace VotaFacil.Domain.Entidades
{
    public class EleitorModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Identificador { get; set; }
        public string EnderecoEthereum { get; set; }

        public EleitorModel(string nome, string cpf)
        {
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(nome), "O nome não pode ser vazio.");
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(cpf), "O nome não pode ser vazio.");


            Id = new Guid();
            Nome = nome;
            Cpf = cpf;
        }

        public bool Autenticar(string identificador, string enderecoEthereum)
        {
            return Identificador == identificador && EnderecoEthereum == enderecoEthereum;
        }
    }
}
