using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VotaFacil.Domain.Model;

namespace VotaFacil.Domain.Entidades
{
    [Table("eleicao")]
    public class EleicaoModel
    {
        [Key, Column("id")] public Guid Id { get; set; }

        [Required, Column("nome"), MaxLength(100)] public string Nome { get; set; }

        [Required, Column("descricao"), MaxLength(500)] public string Descricao { get; set; }

        [Required, Column("inicio")] public DateTime Inicio { get; private set; }

        [Required, Column("fim")] public DateTime Fim { get; private set; }

        // Relação com CandidatoModel
        public ICollection<CandidatoModel> Candidatos { get; set; } = new List<CandidatoModel>();

        // Relação com VotoModel
        public ICollection<VotoModel> Votos { get; set; } = new List<VotoModel>();

        private readonly List<Guid> _votosRegistrados = new List<Guid>();

        public EleicaoModel() { }

        public EleicaoModel(string nome, string descricao, DateTime inicio, DateTime fim)
        {
            if (inicio >= fim)
                throw new ArgumentException("A data de início deve ser anterior à data de fim.");

            Id = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            Inicio = inicio;
            Fim = fim;
        }

        public EleicaoModel(EleitorModel votante, VotoModel opcaoVoto)
        {
            if (!EstaDentroDoPeriodo(DateTime.Now))
                throw new Exception("Fora do período de votação.");

            if (_votosRegistrados.Contains(votante.Id))
                throw new Exception("Votante já votou.");

            // Registrar voto (na vida real, aqui você registraria a transação na blockchain)
            _votosRegistrados.Add(votante.Id);
        }

        public bool EstaDentroDoPeriodo(DateTime dataAtual)
        {
            return dataAtual >= Inicio && dataAtual <= Fim;
        }
    }
}
