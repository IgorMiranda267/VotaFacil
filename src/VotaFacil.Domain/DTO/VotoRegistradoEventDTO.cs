using Nethereum.ABI.FunctionEncoding.Attributes;

namespace VotaFacil.Domain.DTO
{

    [Event("VotoRegistrado")]
    public class VotoRegistradoEventDTO : IEventDTO
    {
        [Parameter("bytes32", "eleitorId", 1, true)]
        public byte[] EleitorId { get; set; }

        [Parameter("bytes32", "opcaoVotoId", 2, true)]
        public byte[] OpcaoVotoId { get; set; }

        [Parameter("string", "hashAnterior", 3, false)]
        public string HashAnterior { get; set; }

        [Parameter("string", "numeroBloco", 4, false)]
        public string NumeroBloco { get; set; }
    }
}
