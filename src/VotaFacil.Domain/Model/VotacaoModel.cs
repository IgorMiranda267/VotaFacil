using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VotaFacil.Domain.Model;

namespace VotaFacil.Domain.Entidades
{
    [Table("votacao")]
    public class VotacaoModel
    {
        [Key, Column("id")] public Guid Id { get; set; }

        [Required, Column("inicio")] public DateTime Inicio { get; private set; }

        [Required, Column("fim")] public DateTime Fim { get; private set; }

        // Relação com CandidatoModel
        public ICollection<CandidatoModel> Candidatos { get; set; } = new List<CandidatoModel>();

        // Relação com VotoModel
        public ICollection<VotoModel> Votos { get; set; } = new List<VotoModel>();

        private readonly List<Guid> _votosRegistrados = new List<Guid>();

        public VotacaoModel() { }

        public VotacaoModel(EleitorModel votante, VotoModel opcaoVoto)
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
