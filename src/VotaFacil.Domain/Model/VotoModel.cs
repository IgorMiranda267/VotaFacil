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

        [Column("eleitor_id"), Required]
        public Guid EleitorId { get; private set; }
        public EleitorModel Eleitor { get; set; }

        [Column("opcao_voto_id"), Required]
        public Guid OpcaoVotoId { get; private set; }

        [Column("votacao_id"), Required]
        public Guid VotacaoId { get; set; }
        public VotacaoModel Votacao { get; set; }

        [Column("candidato_id"), Required]
        public Guid CandidatoId { get; set; }
        public CandidatoModel Candidato { get; set; }

        [Column("data_hora_voto"), Required] public DateTime DataHoraVoto { get; private set; }

        [Column("hash_anterior"), MaxLength(64)] public string HashAnterior { get; private set; }

        [Column("hash_atual"), MaxLength(64)] public string HashAtual { get; private set; }

        public VotoModel(Guid eleitorId, Guid opcaoVotoId, string hashAnterior)
        {
            Id = Guid.NewGuid();
            EleitorId = eleitorId;
            OpcaoVotoId = opcaoVotoId;
            DataHoraVoto = DateTime.UtcNow;
            HashAnterior = hashAnterior;
            HashAtual = GerarHash();
        }

        private string GerarHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{Id}{EleitorId}{OpcaoVotoId}{DataHoraVoto}{HashAnterior}";
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
