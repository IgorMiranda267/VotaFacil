using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using VotaFacil.Domain.Model;

namespace VotaFacil.Domain.Entidades
{
    [Table("voto")]
    public class VotoModel
    {
        [Column("id"), Key] public Guid Id { get; private set; }
        [Column("eleitor_id"), Required] public Guid EleitorId { get; private set; }
        [Column("votacao_id"), Required] public Guid VotacaoId { get; private set; }
        [Column("candidato_id"), Required] public Guid CandidatoId { get; private set; }
        [Column("data_hora_voto"), Required] public DateTime DataHoraVoto { get; private set; }
        [Column("hash_anterior"), MaxLength(200)] public string HashAnterior { get; private set; }
        [Column("hash_atual"), MaxLength(200)] public string HashAtual { get; private set; }
        [Column("numero_bloco"), Required] public string NumeroBloco { get; private set; }

        public EleicaoModel Votacao { get; set; }
        public EleitorModel Eleitor { get; set; }
        public CandidatoModel Candidato { get; set; }

        public VotoModel() { }

        public VotoModel(Guid eleitorId, Guid candidatoId, Guid votacaoId, string hashAnterior, string numeroBloco)
        {
            Id = Guid.NewGuid();
            EleitorId = eleitorId;
            CandidatoId = candidatoId;
            VotacaoId = votacaoId;
            DataHoraVoto = DateTime.UtcNow;
            HashAnterior = hashAnterior;
            HashAtual = GerarHash();
            NumeroBloco = numeroBloco;
        }

        private string GerarHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{Id}{EleitorId}{CandidatoId}{DataHoraVoto}{HashAnterior}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool VerificarIntegridade(string hashAnterior)
        {
            return HashAnterior == hashAnterior && HashAtual == GerarHash();
        }
    }
}
