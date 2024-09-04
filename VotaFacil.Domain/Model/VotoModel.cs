using System.Security.Cryptography;
using System.Text;

namespace VotaFacil.Domain.Entidades
{
    public class VotoModel
    {
        public Guid Id { get; private set; }
        public Guid EleitorId { get; private set; }
        public Guid OpcaoVotoId { get; private set; }
        public DateTime DataHoraVoto { get; private set; }
        public string HashAnterior { get; private set; }
        public string HashAtual { get; private set; }

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
