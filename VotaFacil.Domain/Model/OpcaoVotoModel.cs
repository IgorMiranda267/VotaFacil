using VotaFacil.Domain.Validacao;

namespace VotaFacil.Domain.Entidades
{
    public class OpcaoVotoModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }

        public OpcaoVotoModel(Guid id, string descricao)
        {
            ValidacaoDeExcecaoDominio.When(string.IsNullOrEmpty(descricao), "A descrição do candidato não pode ser vazia.");

            Id = id;
            Descricao = descricao;
        }
    }
}
