using Nethereum.Hex.HexTypes;
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
        [Column("contract_address"), MaxLength(42)] public string ContractAddress { get; set; }
        [Column("transaction_hash"), MaxLength(66)] public string TransactionHash { get; set; }

        // Propriedades para armazenar os valores de BlockNumber e GasUsed como strings
        [Column("block_number")] public string BlockNumberString { get; set; }
        [Column("gas_used")] public string GasUsedString { get; set; }

        [NotMapped]
        public HexBigInteger BlockNumber
        {
            get => new HexBigInteger(BlockNumberString);
            set => BlockNumberString = value.Value.ToString();
        }

        [NotMapped]
        public HexBigInteger GasUsed
        {
            get => new HexBigInteger(GasUsedString);
            set => GasUsedString = value.Value.ToString();
        }

        public ICollection<CandidatoModel> Candidatos { get; set; } = new List<CandidatoModel>(); // Relação com CandidatoModel

        public ICollection<VotoModel> Votos { get; set; } = new List<VotoModel>(); // Relação com VotoModel

        private readonly List<Guid> _votosRegistrados = new List<Guid>();

        public EleicaoModel()
        {
            Nome = string.Empty;
            Descricao = string.Empty;
            ContractAddress = string.Empty;
            TransactionHash = string.Empty;
            BlockNumberString = "0";
            GasUsedString = "0";
        }

        public EleicaoModel(string nome, string descricao, DateTime inicio, DateTime fim, string contractAddress, string transactionHash, HexBigInteger blockNumber, HexBigInteger gasUsed)
        {
            if (inicio >= fim)
                throw new ArgumentException("A data de início deve ser anterior à data de fim.");

            Id = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            Inicio = DateTime.SpecifyKind(inicio, DateTimeKind.Utc);
            Fim = DateTime.SpecifyKind(fim, DateTimeKind.Utc);
            ContractAddress = contractAddress;
            TransactionHash = transactionHash;
            BlockNumber = blockNumber;
            GasUsed = gasUsed;
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
